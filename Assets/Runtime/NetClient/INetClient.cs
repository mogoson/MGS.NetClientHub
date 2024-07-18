/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  INetClient.cs
 *  Description  :  Interface of client to connect remote.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.Work;

namespace MGS.Net
{
    /// <summary>
    /// Interface of client to connect remote.
    /// </summary>
    public interface INetClient<T> : IAsyncWork<T>
    {
        /// <summary>
        /// Remote url string.
        /// </summary>
        string URL { get; }
    }
}