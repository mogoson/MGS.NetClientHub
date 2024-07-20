/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetGetClient.cs
 *  Description  :  Net get client.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Net;

namespace MGS.Net
{
    /// <summary>
    /// Net get client.
    /// </summary>
    public class NetGetClient : NetClient<string>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="headData">Head data of request.</param>
        public NetGetClient(string url, int timeout, IDictionary<string, string> headData = null) : base(url, timeout, headData) { }

        /// <summary>
        /// Do request work.
        /// </summary>
        /// <param name="request"></param>
        protected override string DoRequest(HttpWebRequest request)
        {
            request.Method = "GET";
            return base.DoRequest(request);
        }

        /// <summary>
        /// Read Result from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected override string ReadResult(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}