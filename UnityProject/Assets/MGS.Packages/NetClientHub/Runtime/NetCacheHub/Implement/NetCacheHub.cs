/*************************************************************************
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
        /// Cacher for net data.
        /// </summary>
        public ICacher<string> Cacher { set; get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cacher">Cacher for net data.</param>
        /// <param name="concurrency">Max count of concurrency clients.</param>
        /// <param name="resolver">Net resolver to check retrieable.</param>
        public NetCacheHub(ICacher<string> cacher, int concurrency = 3, INetResolver resolver = null) : base(concurrency, resolver)
        {
            Cacher = cacher;
        }

        /// <summary>
        /// Put url to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <returns></returns>
        public override INetClient Put(string url, int timeout)
        {
            var key = url.GetHashCode().ToString();
            var cache = Cacher.Get(key);
            if (!string.IsNullOrEmpty(cache))
            {
                return new CacheClient(cache);
            }
            return base.Put(url, timeout);
        }

        /// <summary>
        /// Post url and data to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public override INetClient Post(string url, int timeout, string postData)
        {
            var key = url.GetHashCode().ToString();
            var cache = Cacher.Get(key);
            if (!string.IsNullOrEmpty(cache))
            {
                return new CacheClient(cache);
            }
            return base.Post(url, timeout, postData);
        }

        /// <summary>
        /// Dispose all resource.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Cacher.Dispose();
            Cacher = null;
        }

        /// <summary>
        /// On client is done.
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientIsDone(INetClient client)
        {
            if (client.Error == null)
            {
                if (!string.IsNullOrEmpty(client.Result))
                {
                    var key = client.URL.GetHashCode().ToString();
                    Cacher.Set(key, client.Result);
                }
            }
            base.OnClientIsDone(client);
        }
    }
}