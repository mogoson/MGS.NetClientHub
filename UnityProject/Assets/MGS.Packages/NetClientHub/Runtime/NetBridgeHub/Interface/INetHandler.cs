/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  INetHandler.cs
 *  Description  :  Interface of handler to manage net client status.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  03/10/2023
 *  Description  :  Initial development version.
 *************************************************************************/

using System;

namespace MGS.Net
{
    /// <summary>
    /// Interface of handler to manage net client status.
    /// </summary>
    public interface INetHandler : IDisposable
    {
        /// <summary>
        /// Client of handler.
        /// </summary>
        INetClient Client { get; }

        /// <summary>
        /// On speed changed event.
        /// </summary>
        event Action<double> OnSpeedChanged;

        /// <summary>
        /// On progress changed event.
        /// </summary>
        event Action<float> OnProgressChanged;

        /// <summary>
        /// On completed event.
        /// </summary>
        event Action<string, Exception> OnCompleted;

        /// <summary>
        /// Notify status of client.
        /// </summary>
        void NotifyStatus();
    }
}