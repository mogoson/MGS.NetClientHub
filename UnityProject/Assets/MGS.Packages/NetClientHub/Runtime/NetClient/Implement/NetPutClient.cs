/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetPutClient.cs
 *  Description  :  Net put client.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MGS.Net
{
    /// <summary>
    /// Net put client.
    /// </summary>
    public class NetPutClient : NetClient
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        public NetPutClient(string url, int timeout, IDictionary<string, string> headData = null) : base(url, timeout, headData) { }

        /// <summary>
        /// Do request work.
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="url"></param>
        protected override void DoRequest(WebClientEx webClient, string url)
        {
            webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            webClient.DownloadStringAsync(new Uri(url));
        }

        /// <summary>
        /// DownloadStringCompleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Progress = e.Error == null ? 1 : 0;
            Size = Encoding.UTF8.GetByteCount(e.Result);
            Result = e.Result;
            Error = e.Error;
            Close();
        }
    }
}