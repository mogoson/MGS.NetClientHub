/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetCacheClient.cs
 *  Description  :  Client for net cache data.
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
    public class NetCacheClient : NetClient
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="result">Result cache data.</param>
        public NetCacheClient(string url, string result) : base(url, 0)
        {
            Result = result;
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