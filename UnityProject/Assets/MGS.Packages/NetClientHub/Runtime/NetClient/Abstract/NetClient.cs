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
using System.Text;

namespace MGS.Net
{
    /// <summary>
    /// Net client abstract implement.
    /// </summary>
    public abstract class NetClient : INetClient
    {
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
        public string Result { protected set; get; }

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
        protected Dictionary<string, string> headData;

        /// <summary>
        /// WebClient to connect remote.
        /// </summary>
        protected WebClientEx webClient;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        public NetClient(string url, int timeout, Dictionary<string, string> headData = null)
        {
            URL = url;
            Timeout = timeout;
            this.headData = headData;
        }

        /// <summary>
        /// Open client to do net work.
        /// </summary>
        public virtual void Open()
        {
            if (webClient == null)
            {
                Reset();
                webClient = new WebClientEx(Timeout)
                {
                    Encoding = Encoding.UTF8
                };

                AddHeaders(webClient, headData);
                try { DoRequest(webClient, URL); }
                catch (Exception ex)
                {
                    Error = ex;
                    Close();
                }
            }
        }

        /// <summary>
        /// Close client to abort net work.
        /// </summary>
        public virtual void Close()
        {
            if (webClient != null)
            {
                webClient.Dispose();
                webClient = null;
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
        /// Add header data to WebClient.
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="headData"></param>
        protected void AddHeaders(WebClientEx webClient, Dictionary<string, string> headData)
        {
            if (headData == null || headData.Count == 0)
            {
                return;
            }

            foreach (var data in headData)
            {
                webClient.Headers.Add(data.Key, data.Value);
            }
        }

        /// <summary>
        /// Do request work.
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="url"></param>
        protected abstract void DoRequest(WebClientEx webClient, string url);
    }
}