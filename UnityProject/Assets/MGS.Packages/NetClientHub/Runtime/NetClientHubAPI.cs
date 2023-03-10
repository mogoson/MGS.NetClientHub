/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetClientHubAPI.cs
 *  Description  :  API for net client hub.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/22/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;

namespace MGS.Net
{
    /// <summary>
    /// API for net client hub.
    /// </summary>
    public sealed class NetClientHubAPI
    {
        /// <summary>
        /// A global instance for API.
        /// </summary>
        public static readonly INetCacheHub handler;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NetClientHubAPI()
        {
            //A cacher with timeout to cache the result from net client.
            var resultCacher = new TimeoutCacher<string>(100, 5000);

            //A cacher to cache the waiting and working client to reuse for the same url.
            var clientCacher = new Cacher<INetClient>(100);

            //Set tolerable exceptions to NetResolver to check retrieable.
            var tolerables = new List<Type> { typeof(WebException), typeof(TimeoutException) };
            var resolver = new NetResolver(3, tolerables);

            //In your case, leave the arg as null if you dont need the cache or retry ability.
            handler = new NetCacheHub(resultCacher, clientCacher, 3, resolver);//Thread work async, do not notify status.
            //handler = new NetBridgeHub(resultCacher, clientCacher, 3, resolver);//Thread work async, invoke the Update method to notify status in your thread.
            //handler = new NetMonoHub(resultCacher, clientCacher, 3, resolver);//Thread work async, notify status in unity main thread.
        }
    }
}