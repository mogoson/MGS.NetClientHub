/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetClientHub.cs
 *  Description  :  Hub to manage net clients.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using MGS.Cachers;
using MGS.Work;

namespace MGS.Net
{
    /// <summary>
    /// Hub to manage net clients.
    /// </summary>
    public class NetClientHub : AsyncWorkMonoHub, INetClientHub
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="concurrency">Max count of concurrency clients.</param>
        /// <param name="resolver">Net resolver to check retrieable.</param>
        public NetClientHub(ICacher<object> resultCacher = null,
            ICacher<IAsyncWork> workCacher = null, int concurrency = 3, IWorkResolver resolver = null)
            : base(resultCacher, workCacher, concurrency, resolver) { }

        /// <summary>
        /// Get url to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public IAsyncWorkHandler<string> GetAsync(string url, int timeout, IDictionary<string, string> headData = null)
        {
            var client = new NetGetClient(url, timeout, headData) { DontSetDoneIfError = true };
            return EnqueueWork(client);
        }

        /// <summary>
        /// Post url and data to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public IAsyncWorkHandler<string> PostAsync(string url, int timeout, string postData, IDictionary<string, string> headData = null)
        {
            var client = new NetPostClient(url, timeout, postData, headData) { DontSetDoneIfError = true };
            return EnqueueWork(client);
        }

        /// <summary>
        /// Download file from server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public IAsyncWorkHandler<string> DownloadAsync(string url, int timeout, string filePath, IDictionary<string, string> headData = null)
        {
            var client = new NetFileClient(url, timeout, filePath, headData) { DontSetDoneIfError = true };
            return EnqueueWork(client);
        }
    }
}