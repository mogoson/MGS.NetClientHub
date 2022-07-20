/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  WebClientEx.cs
 *  Description  :  WebClient extention.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Net;

namespace MGS.Net
{
    /// <summary>
    /// WebClient extention.
    /// </summary>
    public class WebClientEx : WebClient
    {
        /// <summary>
        /// Request timeout(ms).
        /// </summary>
        public int Timeout { set; get; }

        /// <summary>
        /// Start range(byte).
        /// </summary>
        public long Range { set; get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="timeout">Request timeout(ms).</param>
        /// <param name="range">Start range(byte).</param>
        public WebClientEx(int timeout, long range = 0)
        {
            Timeout = timeout;
            Range = range;
        }

        /// <summary>
        /// GetWebRequest.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request == null)
            {
                return request;
            }

            request.Timeout = Timeout;

            var httpRequest = request as HttpWebRequest;
            if (httpRequest == null)
            {
                return request;
            }

            httpRequest.Timeout = Timeout;
            httpRequest.ReadWriteTimeout = Timeout;

            if (Range > 0)
            {
#if UNITY_5
                httpRequest.AddRange((int)Range);
#else
                httpRequest.AddRange(Range);
#endif
                httpRequest.Accept = "*/*";
            }

            return request;
        }
    }
}