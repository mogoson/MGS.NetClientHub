/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetClient.cs
 *  Description  :  Net client abstract implement.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace MGS.Net
{
    /// <summary>
    /// Net client abstract implement.
    /// </summary>
    public abstract class NetClient : INetClient
    {
        /// <summary>
        /// Key of client.
        /// </summary>
        public string Key { protected set; get; }

        /// <summary>
        /// Remote url string.
        /// </summary>
        public string URL { protected set; get; }

        /// <summary>
        /// Timeout(ms) of request.
        /// </summary>
        public int Timeout { protected set; get; }

        /// <summary>
        /// Work of client is done?
        /// </summary>
        public bool IsDone { protected set; get; }

        /// <summary>
        /// Data size(byte).
        /// </summary>
        public long Size { protected set; get; }

        /// <summary>
        /// Request speed(byte/s).
        /// </summary>
        public double Speed { protected set; get; }

        /// <summary>
        /// Progress(0~1) of work.
        /// </summary>
        public float Progress { protected set; get; }

        /// <summary>
        /// Result of work.
        /// </summary>
        public object Result { protected set; get; }

        /// <summary>
        /// Error of work.
        /// </summary>
        public Exception Error { protected set; get; }

        /// <summary>
        /// Dont set the property "IsDone" as true if error?
        /// </summary>
        public bool DontSetDoneIfError { set; get; }

        /// <summary>
        /// Head data of request.
        /// </summary>
        protected IDictionary<string, string> headData;

        /// <summary>
        /// HttpWebRequest to connect remote.
        /// </summary>
        protected HttpWebRequest request;

        /// <summary>
        /// Thread to do net work.
        /// </summary>
        protected Thread thread;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        public NetClient(string url, int timeout, IDictionary<string, string> headData = null)
        {
            Key = GetKey(url);
            URL = url;
            Timeout = timeout;
            this.headData = headData;
        }

        /// <summary>
        /// Get key for client.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetKey(string url)
        {
            return url.GetHashCode().ToString();
        }

        /// <summary>
        /// Open client to do net work.
        /// </summary>
        public virtual void Open()
        {
            if (request == null)
            {
                Reset();

                request = CreateWebRequest(URL);
                AddHeaders(request, headData);

                thread = CreateRequestThread(request);
                thread.Start();
            }
        }

        /// <summary>
        /// Close client to abort net work.
        /// </summary>
        public virtual void Close()
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
                thread = null;
            }

            if (request != null)
            {
                request.Abort();
                request = null;
            }

            if (Error == null || !DontSetDoneIfError)
            {
                IsDone = true;
            }
        }

        /// <summary>
        /// Dispose all resource.
        /// </summary>
        public void Dispose()
        {
            Close();
            Reset();
            headData = null;
        }

        /// <summary>
        /// Reset status.
        /// </summary>
        protected void Reset()
        {
            IsDone = false;
            Size = 0;
            Speed = 0;
            Progress = 0;
            Result = null;
            Error = null;
        }

        /// <summary>
        /// Create HttpWebRequest for url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected HttpWebRequest CreateWebRequest(string url)
        {
            var webRequest = WebRequest.CreateHttp(url);
            {
                webRequest.Timeout = Timeout;
                webRequest.ReadWriteTimeout = Timeout;
            }
            return webRequest;
        }

        /// <summary>
        /// Add header data to WebClient.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="headData"></param>
        protected void AddHeaders(HttpWebRequest request, IDictionary<string, string> headData)
        {
            if (headData == null || headData.Count == 0)
            {
                return;
            }

            foreach (var data in headData)
            {
                if (data.Key == "Content-Type")
                {
                    request.ContentType = data.Value;
                    continue;
                }
                request.Headers.Add(data.Key, data.Value);
            }
        }

        /// <summary>
        /// Create Thread to do web request work.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected Thread CreateRequestThread(HttpWebRequest request)
        {
            return new Thread(() =>
            {
                try
                {
                    DoRequest(request);
                }
                catch (Exception ex)
                {
                    Error = ex;
                    Close();
                }
            })
            { IsBackground = true };
        }

        /// <summary>
        /// Do request work.
        /// </summary>
        /// <param name="request"></param>
        protected virtual void DoRequest(HttpWebRequest request)
        {
            var response = request.GetResponse();
            Size = response.ContentLength;

            var encoding = response.Headers.Get("Content-Encoding");
            var responseStream = response.GetResponseStream();
            if (encoding == "gzip")
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            Result = ReadResult(responseStream);
            responseStream.Close();

            Progress = 1.0f;
            IsDone = true;

            thread = null;
            request = null;
        }

        /// <summary>
        /// Read Result from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected abstract object ReadResult(Stream stream);
    }
}