/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetPostClient.cs
 *  Description  :  Net post client.
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
    /// Net post client.
    /// </summary>
    public class NetPostClient : NetClient
    {
        /// <summary>
        /// Post data of request.
        /// </summary>
        public string PostData { protected set; get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        /// <param name="postData">Post data of request.</param>
        public NetPostClient(string url, int timeout, Dictionary<string, string> headData, string postData) : base(url, timeout, headData)
        {
            PostData = postData;
        }

        /// <summary>
        /// Do request work.
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="url"></param>
        protected override void DoRequest(WebClientEx webClient, string url)
        {
            webClient.UploadDataCompleted += WebClient_UploadDataCompleted;
            webClient.UploadDataAsync(new Uri(url), Encoding.UTF8.GetBytes(PostData));
        }

        /// <summary>
        /// UploadDataCompleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebClient_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            Progress = e.Error == null ? 1 : 0;
            Size = e.Result.LongLength;
            Result = Encoding.UTF8.GetString(e.Result);
            Error = e.Error;
            Close();
        }
    }
}