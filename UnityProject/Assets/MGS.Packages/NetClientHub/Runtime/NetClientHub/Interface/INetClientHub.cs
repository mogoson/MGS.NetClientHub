/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  INetClientHub.cs
 *  Description  :  Interface of hub to manage net clients.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;

namespace MGS.Net
{
    /// <summary>
    /// Interface of hub to manage net clients.
    /// </summary>
    public interface INetClientHub : IDisposable
    {
        /// <summary>
        /// Max count of concurrency clients.
        /// </summary>
        int Concurrency { set; get; }

        /// <summary>
        /// Net resolver to check retrieable.
        /// </summary>
        INetResolver Resolver { set; get; }

        /// <summary>
        /// Put url to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        INetClient PutAsync(string url, int timeout, IDictionary<string, string> headData = null);

        /// <summary>
        /// Post url and data to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        INetClient PostAsync(string url, int timeout, string postData, IDictionary<string, string> headData = null);

        /// <summary>
        /// Download file from server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        INetClient DownloadAsync(string url, int timeout, string filePath, IDictionary<string, string> headData = null);

        /// <summary>
        /// Discard net clients.
        /// </summary>
        /// <param name="workings">Discard the working clients?</param>
        /// <param name="waitings">Discard the waiting clients?</param>
        void Discard(bool workings, bool waitings);
    }
}