/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  INetMonoHub.cs
 *  Description  :  Interface for hub to manage net clients and cache net
 *                  data, and unity main thread notify status.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  03/10/2023
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.Net
{
    /// <summary>
    /// Interface for hub to manage net clients and cache net data,
    /// and unity main thread notify status.
    /// </summary>
    public interface INetMonoHub : INetBridgeHub { }
}