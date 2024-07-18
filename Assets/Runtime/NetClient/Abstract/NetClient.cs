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

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using MGS.Work;

namespace MGS.Net
{
    /// <summary>
    /// Net client abstract implement.
    /// </summary>
    public abstract class NetClient<T> : AsyncWork<T>, INetClient<T>
    {
        /// <summary>
        /// Remote url string.
        /// </summary>
        public string URL { protected set; get; }

        /// <summary>
        /// 
        /// </summary>
        public bool DontSetDoneIfError { set; get; }

        /// <summary>
        /// HttpWebRequest to connect remote.
        /// </summary>
        protected HttpWebRequest request;

        /// <summary>
        /// Head data of request.
        /// </summary>
        protected IDictionary<string, string> headData;

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
        /// 
        /// </summary>
        protected override void OnExecute()
        {
            if (request == null)
            {
                request = CreateWebRequest(URL);
                AddHeaders(request, headData);
                DoRequest(request);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnFinally()
        {
            base.OnFinally();
            request = null;
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
                if (data.Key == "ContentType")
                {
                    request.ContentType = data.Value;
                    continue;
                }
                if (data.Key == "Accept")
                {
                    request.Accept = data.Value;
                    continue;
                }
                if (data.Key == "UserAgent")
                {
                    request.UserAgent = data.Value;
                    continue;
                }
                request.Headers.Add(data.Key, data.Value);
            }
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
        }

        /// <summary>
        /// Read Result from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected abstract T ReadResult(Stream stream);
    }
}