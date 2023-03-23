/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  INetResolver.cs
 *  Description  :  Interface of net resolver to check retrieable.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;

namespace MGS.Net
{
    /// <summary>
    /// Interface of net resolver to check retrieable.
    /// </summary>
    public interface INetResolver : IDisposable
    {
        /// <summary>
        /// Check client is retrieable?
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        bool Retrieable(INetClient client);

        /// <summary>
        /// Clear the history of client.
        /// </summary>
        /// <param name="client"></param>
        void Clear(INetClient client);

        /// <summary>
        /// Clear the history of all clients.
        /// </summary>
        void Clear();
    }
}