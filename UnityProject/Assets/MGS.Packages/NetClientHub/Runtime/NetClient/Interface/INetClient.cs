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

using System;

namespace MGS.Net
{
    /// <summary>
    /// Interface of client to connect remote.
    /// </summary>
    public interface INetClient : IDisposable
    {
        /// <summary>
        /// Remote url string.
        /// </summary>
        string URL { get; }

        /// <summary>
        /// Timeout(ms) of request.
        /// </summary>
        int Timeout { get; }

        /// <summary>
        /// Work of client is done?
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// Data size(byte).
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Request speed(byte/s).
        /// </summary>
        double Speed { get; }

        /// <summary>
        /// Progress(0~1) of work.
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// Result of work.
        /// </summary>
        string Result { get; }

        /// <summary>
        /// Error of work.
        /// </summary>
        Exception Error { get; }

        /// <summary>
        /// Open client to do net work.
        /// </summary>
        void Open();

        /// <summary>
        /// Close client to abort net work.
        /// </summary>
        void Close();
    }
}