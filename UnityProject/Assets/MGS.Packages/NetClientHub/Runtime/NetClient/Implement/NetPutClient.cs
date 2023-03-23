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
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            webClient.DownloadStringAsync(new Uri(url));
        }

        /// <summary>
        /// DownloadProgressChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage * 0.01f;
        }

        /// <summary>
        /// DownloadStringCompleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Size = Encoding.UTF8.GetByteCount(e.Result);
            Result = e.Result;
            Error = e.Error;
            Close();
        }
    }
}