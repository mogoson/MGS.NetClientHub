/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  WorkHubHandler.cs
 *  Description  :  Handler for async work hub API.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/22/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.Cachers;
using System;
using System.Collections.Generic;
using System.Net;

namespace MGS.Work
{
    /// <summary>
    /// Handler for async work hub API.
    /// </summary>
    public sealed class WorkHubHandler
    {
        /// <summary>
        /// A global instance for API.
        /// </summary>
        public static readonly IAsyncWorkStatusHub API;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static WorkHubHandler()
        {
            //A cacher with timeout to cache the result from work.
            var resultCacher = new TimeoutCacher<object>(100, 5000);

            //A cacher to cache the waiting and working work to reuse for the same url.
            var workCacher = new Cacher<IAsyncWork>(100);

            //Set tolerable exceptions to WorkResolver to check retrieable.
            var tolerables = new List<Type> { typeof(WebException), typeof(TimeoutException) };
            var resolver = new WorkResolver(3, tolerables);

            //In your case, leave the arg as null if you dont need the cache or retry ability.
            //Thread work async, do not notify status.
            //API = new AsyncWorkCacheHub(resultCacher, workCacher, 10, resolver);

#if UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID
            //Thread work async, notify status in unity main thread.
            API = new AsyncWorkMonoHub(resultCacher, workCacher, 10, resolver);
#else
            //Thread work async, invoke the Update method to notify status in your thread.
            API = new AsyncWorkStatusHub(resultCacher, workCacher, 10, resolver);
#endif
        }
    }
}