/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetWorkHubHandler.cs
 *  Description  :  Handler of net work hub.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;

namespace MGS.Work.Net
{
    /// <summary>
    /// Handler of net work hub.
    /// </summary>
    public sealed class NetWorkHubHandler
    {
        /// <summary>
        /// Get from remote async.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        public static IAsyncWorkHandler GetAsync(string url, int timeout, IDictionary<string, string> headData = null)
        {
            var work = new NetGetWork(url, timeout, headData);
            //return WorkHubHandler.API.EnqueueWork(work);
            return null;
        }

        /// <summary>
        /// Post from remote async.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData">Post data of request.</param>
        /// <param name="headData">Head data of request.</param>
        public static IAsyncWorkHandler PostAsync(string url, int timeout, string postData, IDictionary<string, string> headData = null)
        {
            var work = new NetPostWork(url, timeout, postData, headData);
            //return WorkHubHandler.API.EnqueueWork(work);
            return null;
        }

        /// <summary>
        /// Download from remote async.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath">Path of local file.</param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public static IAsyncWorkHandler DownloadAsync(string url, int timeout, string filePath, IDictionary<string, string> headData = null)
        {
            var work = new NetFileWork(url, timeout, filePath, headData);
            //return WorkHubHandler.API.EnqueueWork(work);
            return null;
        }
    }
}