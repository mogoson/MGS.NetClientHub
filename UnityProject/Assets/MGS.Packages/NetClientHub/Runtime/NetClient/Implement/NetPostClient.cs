/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetPostClient.cs
 *  Description  :  Net post client.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MGS.Net
{
    /// <summary>
    /// Net post client.
    /// </summary>
    public class NetPostClient : NetClient
    {
        /// <summary>
        /// Post data of request.
        /// </summary>
        /// 
        public string PostData { protected set; get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData">Post data of request.</param>
        /// <param name="headData">Head data of request.</param>
        public NetPostClient(string url, int timeout, string postData, IDictionary<string, string> headData = null) : base(url, timeout, headData)
        {
            Key = GetKey(url, postData);
            PostData = postData;
        }

        /// <summary>
        /// Get key for client.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetKey(string url, string postData)
        {
            return (url + postData).GetHashCode().ToString();
        }

        /// <summary>
        /// Do request work.
        /// </summary>
        /// <param name="request"></param>
        protected override void DoRequest(HttpWebRequest request)
        {
            request.Method = "POST";
            var requestStream = request.GetRequestStream();

            var postBuffer = Encoding.UTF8.GetBytes(PostData);
            requestStream.Write(postBuffer, 0, postBuffer.Length);
            requestStream.Close();
            Progress = 0.5f;

            base.DoRequest(request);
        }

        /// <summary>
        /// Read Result from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected override object ReadResult(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}