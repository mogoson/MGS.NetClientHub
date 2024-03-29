﻿/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetCacheHub.cs
 *  Description  :  Hub to manage net clients and cache net data.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using System.IO;

namespace MGS.Net
{
    /// <summary>
    /// Hub to manage net clients and cache net data.
    /// NetCacheHub must work with a Cacher;
    /// Use NetClientHub if not need cache ability.
    /// </summary>
    public class NetCacheHub : NetClientHub, INetCacheHub
    {
        /// <summary>
        /// Cacher for net result.
        /// </summary>
        public ICacher<object> ResultCacher { set; get; }

        /// <summary>
        /// Cacher for net client.
        /// </summary>
        public ICacher<INetClient> ClientCacher { set; get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="resultCacher">Cacher for net result.</param>
        /// <param name="clientCacher">Cacher for net client.</param>
        /// <param name="concurrency">Max count of concurrency clients.</param>
        /// <param name="resolver">Net resolver to check retrieable.</param>
        public NetCacheHub(ICacher<object> resultCacher = null, ICacher<INetClient> clientCacher = null,
            int concurrency = 3, INetResolver resolver = null) : base(concurrency, resolver)
        {
            ResultCacher = resultCacher;
            ClientCacher = clientCacher;
        }

        /// <summary>
        /// Get url to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public override INetClient GetAsync(string url, int timeout, IDictionary<string, string> headData = null)
        {
            var key = NetClient.GetKey(url);
            var client = GetCacheClient(url, key);
            if (client == null)
            {
                client = base.GetAsync(url, timeout, headData);
                SetCacheClient(key, client);
            }
            return client;
        }

        /// <summary>
        /// Post url and data to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public override INetClient PostAsync(string url, int timeout, string postData, IDictionary<string, string> headData = null)
        {
            var key = NetPostClient.GetKey(url, postData);
            var client = GetCacheClient(url, key);
            if (client == null)
            {
                client = base.PostAsync(url, timeout, postData, headData);
                SetCacheClient(key, client);
            }
            return client;
        }

        /// <summary>
        /// Download file from server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath"></param>
        /// <param name="headData">Head data of request.</param>
        /// <returns></returns>
        public override INetClient DownloadAsync(string url, int timeout, string filePath, IDictionary<string, string> headData = null)
        {
            var key = NetClient.GetKey(url);
            var client = GetCacheClient(url, key, true);
            if (client == null)
            {
                client = base.DownloadAsync(url, timeout, filePath, headData);
                SetCacheClient(key, client);
            }
            return client;
        }

        /// <summary>
        /// Clear cache resources.
        /// </summary>
        /// <param name="workings">Clear the working clients?</param>
        /// <param name="waitings">Clear the waiting clients?</param>
        public override void Clear(bool workings, bool waitings)
        {
            base.Clear(workings, waitings);

            ResultCacher.Clear();
            ClientCacher.Clear();
        }

        /// <summary>
        /// Dispose all resource.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            ResultCacher.Dispose();
            ResultCacher = null;

            ClientCacher.Dispose();
            ClientCacher = null;
        }

        /// <summary>
        /// On client is done.
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientIsDone(INetClient client)
        {
            if (client.Result != null)
            {
                SetCacheResult(client.Key, client.Result);
            }

            RemoveCacheClient(client.Key);
            base.OnClientIsDone(client);
        }

        /// <summary>
        /// Get client from one of the cachers(ResultCacher/ClientCacher).
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="checkFile"></param>
        /// <returns></returns>
        protected INetClient GetCacheClient(string url, string key, bool checkFile = false)
        {
            var result = GetCacheResult(key);
            if (result != null)
            {
                if (!checkFile || File.Exists(result as string))
                {
                    return new NetCacheClient(url, result);
                }
            }

            return GetCacheClient(key);
        }

        /// <summary>
        /// Get result from ResultCacher.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object GetCacheResult(string key)
        {
            if (ResultCacher == null)
            {
                return null;
            }
            return ResultCacher.Get(key);
        }

        /// <summary>
        /// Set result to ResultCacher.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="result"></param>
        protected void SetCacheResult(string key, object result)
        {
            if (ResultCacher != null)
            {
                ResultCacher.Set(key, result);
            }
        }

        /// <summary>
        /// Get client from ClientCacher.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected INetClient GetCacheClient(string key)
        {
            if (ClientCacher == null)
            {
                return null;
            }
            return ClientCacher.Get(key);
        }

        /// <summary>
        /// Set client to ClientCacher.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected void SetCacheClient(string key, INetClient client)
        {
            if (ClientCacher != null)
            {
                ClientCacher.Set(key, client);
            }
        }

        /// <summary>
        /// Remove client from ClientCacher.
        /// </summary>
        /// <param name="key"></param>
        protected void RemoveCacheClient(string key)
        {
            if (ClientCacher != null)
            {
                ClientCacher.Remove(key);
            }
        }
    }
}