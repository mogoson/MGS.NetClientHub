/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  INetCacheHub.cs
 *  Description  :  Interface of hub to manage net clients and cache net data.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.Cachers;

namespace MGS.Net
{
    /// <summary>
    /// Interface of hub to manage net clients and cache net data.
    /// </summary>
    public interface INetCacheHub : INetClientHub
    {
        /// <summary>
        /// Cacher for net client.
        /// </summary>
        ICacher<INetClient> ClientCacher { set; get; }

        /// <summary>
        /// Cacher for net result.
        /// </summary>
        ICacher<object> ResultCacher { set; get; }
    }
}