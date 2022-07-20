/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  CacheClient.cs
 *  Description  :  Client for cache data.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.Net
{
    /// <summary>
    /// Client for cache data.
    /// </summary>
    public class CacheClient : NetClient
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cache">Cache data.</param>
        public CacheClient(string cache) : base(null, 0)
        {
            Result = cache;
            IsDone = true;
        }

        /// <summary>
        /// Open client(do nothing in this case).
        /// </summary>
        public override void Open() { }

        /// <summary>
        /// Close client(do nothing in this case).
        /// </summary>
        public override void Close() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="url"></param>
        protected override void DoRequest(WebClientEx webClient, string url) { }
    }
}