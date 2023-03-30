/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  IAsyncWorkHandler.cs
 *  Description  :  Interface of handler to manage work status.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  03/10/2023
 *  Description  :  Initial development version.
 *************************************************************************/

using System;

namespace MGS.Work
{
    /// <summary>
    /// Interface of handler to manage work status.
    /// </summary>
    public interface IAsyncWorkHandler : IDisposable
    {
        /// <summary>
        /// Work of handler.
        /// </summary>
        IAsyncWork Work { get; }

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
        event Action<object, Exception> OnCompleted;

        /// <summary>
        /// Notify status of work.
        /// </summary>
        void NotifyStatus();
    }
}