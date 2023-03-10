﻿/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetBridgeHub.cs
 *  Description  :  Hub to manage net clients and cache net
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
    /// Hub to manage net clients and cache net data,
    /// and let other thread notify status.
    /// </summary>
    public class NetBridgeHub : NetCacheHub, INetBridgeHub
    {
        /// <summary>
        /// Handlers for clients.
        /// </summary>
        protected Dictionary<string, INetHandler> handlers = new Dictionary<string, INetHandler>();

        /// <summary>
        /// Temp list.
        /// </summary>
        protected List<string> temps = new List<string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resultCacher">Cacher for net result.</param>
        /// <param name="clientCacher">Cacher for net client.</param>
        /// <param name="concurrency">Max count of concurrency clients.</param>
        /// <param name="resolver">Net resolver to check retrieable.</param>
        public NetBridgeHub(ICacher<string> resultCacher = null,
            ICacher<INetClient> clientCacher = null, int concurrency = 3, INetResolver resolver = null)
            : base(resultCacher, clientCacher, concurrency, resolver) { }

        /// <summary>
        /// Update to notify status.
        /// </summary>
        public void Update()
        {
            temps.Clear();
            foreach (var handler in handlers.Values)
            {
                handler.NotifyStatus();
                if (handler.Client.IsDone)
                {
                    temps.Add(handler.Client.Key);
                }
            }
            foreach (var key in temps)
            {
                handlers.Remove(key);
            }
        }

        /// <summary>
        /// Put url to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public new INetHandler PutAsync(string url, int timeout, IDictionary<string, string> headData = null)
        {
            var client = base.PutAsync(url, timeout, headData);
            return GetHandler(client);
        }

        /// <summary>
        /// Post url and data to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public new INetHandler PostAsync(string url, int timeout, string postData, IDictionary<string, string> headData = null)
        {
            var client = base.PostAsync(url, timeout, postData, headData);
            return GetHandler(client);
        }

        /// <summary>
        /// Download file from server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public new INetHandler DownloadAsync(string url, int timeout, string filePath, IDictionary<string, string> headData = null)
        {
            var client = base.DownloadAsync(url, timeout, filePath, headData);
            return GetHandler(client);
        }

        /// <summary>
        /// Dispose all resource.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            handlers = null;
            temps = null;
        }

        /// <summary>
        /// Get handler for client.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected INetHandler GetHandler(INetClient client)
        {
            if (handlers.ContainsKey(client.Key))
            {
                return handlers[client.Key];
            }

            var handler = new NetHandler(client);
            handlers.Add(client.Key, handler);
            return handler;
        }
    }
}