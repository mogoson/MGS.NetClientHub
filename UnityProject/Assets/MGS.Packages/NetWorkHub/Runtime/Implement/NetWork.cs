﻿/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetWork.cs
 *  Description  :  Net work abstract implement.
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

namespace MGS.Work.Net
{
    /// <summary>
    /// Net work abstract implement.
    /// </summary>
    public abstract class NetWork : AsyncWork, INetWork
    {
        /// <summary>
        /// Remote url string.
        /// </summary>
        public string URL { protected set; get; }

        /// <summary>
        /// Head data of request.
        /// </summary>
        protected IDictionary<string, string> headData;

        /// <summary>
        /// HttpWebRequest to connect remote.
        /// </summary>
        protected HttpWebRequest request;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        public NetWork(string url, int timeout, IDictionary<string, string> headData = null)
        {
            Key = GetKey(url);
            URL = url;
            Timeout = timeout;
            this.headData = headData;
        }

        /// <summary>
        /// Get key for work.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetKey(string url)
        {
            return url.GetHashCode().ToString();
        }

        /// <summary>
        /// Execute work operation.
        /// </summary>
        public override void Execute()
        {
            request = CreateWebRequest(URL);
            AddHeaders(request, headData);
            ExecuteRequest(request);
        }

        /// <summary>
        /// Abort work.
        /// </summary>
        public override void AbortAsync()
        {
            if (request != null)
            {
                request.Abort();
                request = null;
            }
            base.AbortAsync();
        }

        /// <summary>
        /// Dispose all resource.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            headData = null;
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
        /// Add header data to WebWork.
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
        /// Execute net request.
        /// </summary>
        /// <param name="request"></param>
        protected virtual void ExecuteRequest(HttpWebRequest request)
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
        }

        /// <summary>
        /// On work finally.
        /// (Should override this method to set flag or clear ref object Only!)
        /// </summary>
        protected override void OnFinally()
        {
            base.OnFinally();
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