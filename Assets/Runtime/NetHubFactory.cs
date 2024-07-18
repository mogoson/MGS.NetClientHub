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
using MGS.Work;

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
        public static INetClientHub CreateNetClientHub(int concurrency = 10,
            int retryTimes = 3, ICollection<Type> tolerables = null,
            int maxCacheCount = 100, int cacheTimeout = 5000)
        {
            var resultCacher = WorkHubFactory.CreateCacher<object>(maxCacheCount, cacheTimeout);
            var workCacher = WorkHubFactory.CreateCacher<IAsyncWork>(maxCacheCount);
            var resolver = WorkHubFactory.CreateResolver(retryTimes, tolerables);
            return new NetClientHub(resultCacher, workCacher, concurrency, resolver);
        }
    }
}