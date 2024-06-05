/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetHubFactory.cs
 *  Description  :  Factory for net client hub.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/22/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using MGS.Caches;

namespace MGS.Net
{
    /// <summary>
    /// Factory for net client hub.
    /// </summary>
    public sealed class NetHubFactory
    {
        /// <summary>
        /// Creates a network mono hub with the specified parameters.
        /// </summary>
        /// <param name="maxCache">The maximum number of items to cache.</param>
        /// <param name="cacheTimeout">The timeout value in milliseconds for caching the result.</param>
        /// <param name="concurrency">The maximum number of concurrent network operations.</param>
        /// <param name="retryCount">The number of times to retry the network operation.</param>
        /// <param name="tolerables">The collection of exception types that are considered tolerable and can be retried.</param>
        /// <returns>An instance of the network mono hub.</returns>
        public static INetMonoHub CreateNetMonoHub(int maxCache = 100, int cacheTimeout = 1000,
            int concurrency = 3, int retryCount = 3, ICollection<Type> tolerables = null)
        {
            var resolver = CreateNetResolver(retryCount, tolerables);
            CreateNetCacher(maxCache, cacheTimeout, out ICacher<INetClient> clientCacher, out ICacher<object> resultCacher);
            return new NetMonoHub(clientCacher, resultCacher, concurrency, resolver);
        }

        /// <summary>
        /// Creates a network bridge hub with the specified parameters.
        /// </summary>
        /// <param name="maxCache">The maximum number of items to cache.</param>
        /// <param name="cacheTimeout">The timeout value in milliseconds for caching the result.</param>
        /// <param name="concurrency">The maximum number of concurrent network operations.</param>
        /// <param name="retryCount">The number of times to retry the network operation.</param>
        /// <param name="tolerables">The collection of exception types that are considered tolerable and can be retried.</param>
        /// <returns>An instance of the network bridge hub.</returns>
        public static INetBridgeHub CreateNetBridgeHub(int maxCache = 100, int cacheTimeout = 1000,
            int concurrency = 3, int retryCount = 3, ICollection<Type> tolerables = null)
        {
            var resolver = CreateNetResolver(retryCount, tolerables);
            CreateNetCacher(maxCache, cacheTimeout, out ICacher<INetClient> clientCacher, out ICacher<object> resultCacher);
            return new NetBridgeHub(clientCacher, resultCacher, concurrency, resolver);
        }

        /// <summary>
        /// Creates a network cache hub with the specified parameters.
        /// </summary>
        /// <param name="maxCache">The maximum number of items to cache.</param>
        /// <param name="cacheTimeout">The timeout value in milliseconds for caching the result.</param>
        /// <param name="concurrency">The maximum number of concurrent network operations.</param>
        /// <param name="retryCount">The number of times to retry the network operation.</param>
        /// <param name="tolerables">The collection of exception types that are considered tolerable and can be retried.</param>
        /// <returns>An instance of the network cache hub.</returns>
        public static INetCacheHub CreateNetCacheHub(int maxCache = 100, int cacheTimeout = 1000,
            int concurrency = 3, int retryCount = 3, ICollection<Type> tolerables = null)
        {
            var resolver = CreateNetResolver(retryCount, tolerables);
            CreateNetCacher(maxCache, cacheTimeout, out ICacher<INetClient> clientCacher, out ICacher<object> resultCacher);
            return new NetCacheHub(clientCacher, resultCacher, concurrency, resolver);
        }

        /// <summary>
        /// Creates a network client hub with the specified concurrency, retry count, and tolerable exception types.
        /// </summary>
        /// <param name="concurrency">The maximum number of concurrent network operations.</param>
        /// <param name="retryCount">The number of times to retry the network operation.</param>
        /// <param name="tolerables">The collection of exception types that are considered tolerable and can be retried.</param>
        /// <returns>An instance of the network client hub.</returns>
        public static INetClientHub CreateNetClientHub(int concurrency = 3, int retryCount = 3, ICollection<Type> tolerables = null)
        {
            var resolver = CreateNetResolver(retryCount, tolerables);
            return new NetClientHub(concurrency, resolver);
        }

        /// <summary>
        /// Creates the network cacher with the specified parameters.
        /// </summary>
        /// <param name="maxCache">The maximum number of items to cache.</param>
        /// <param name="timeout">The timeout value in milliseconds for caching the result.</param>
        /// <param name="clientCacher">The cacher for caching the waiting and working client.</param>
        /// <param name="resultCacher">The cacher for caching the result from the net client.</param>
        private static void CreateNetCacher(int maxCache, int timeout, out ICacher<INetClient> clientCacher, out ICacher<object> resultCacher)
        {
            clientCacher = null;
            resultCacher = null;

            if (maxCache <= 0)
            {
                return;
            }

            //A cacher to cache the waiting and working client to reuse for the same url.
            clientCacher = new Cacher<INetClient>(maxCache);

            if (timeout > 0)
            {
                //A cacher with timeout to cache the result from net client.
                resultCacher = new TimeoutCacher<object>(maxCache, timeout);
            }
            else
            {
                //A cacher to cache the result from net client.
                resultCacher = new Cacher<object>(maxCache);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="INetResolver"/> with the specified parameters.
        /// </summary>
        /// <param name="times">The number of times to retry the network operation.</param>
        /// <param name="tolerables">The collection of exception types that are considered tolerable and can be retried.</param>
        /// <returns>An instance of <see cref="INetResolver"/>.</returns>
        private static INetResolver CreateNetResolver(int times, ICollection<Type> tolerables)
        {
            if (times <= 0)
            {
                return null;
            }

            if (tolerables == null)
            {
                // Set tolerable exceptions to NetResolver to check retrievable.
                tolerables = new List<Type> { typeof(WebException), typeof(TimeoutException) };
            }
            return new NetResolver(times, tolerables);
        }
    }
}