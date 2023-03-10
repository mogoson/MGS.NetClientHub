/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  INetBridgeHub.cs
 *  Description  :  Interface of hub to manage net clients and cache net
 *                  data, and let other thread notify status.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  03/10/2023
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;

namespace MGS.Net
{
    /// <summary>
    /// Interface of hub to manage net clients and cache net data,
    /// and let other thread notify status.
    /// </summary>
    public interface INetBridgeHub : INetCacheHub
    {
        /// <summary>
        /// Update to notify status.
        /// </summary>
        void Update();

        /// <summary>
        /// Put url to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        new INetHandler PutAsync(string url, int timeout, IDictionary<string, string> headData = null);

        /// <summary>
        /// Post url and data to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        new INetHandler PostAsync(string url, int timeout, string postData, IDictionary<string, string> headData = null);

        /// <summary>
        /// Download file from server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        new INetHandler DownloadAsync(string url, int timeout, string filePath, IDictionary<string, string> headData = null);
    }
}