/*************************************************************************
 *  Copyright © 2023 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetHandler.cs
 *  Description  :  Handler to manage net client status.
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
    /// Handler to manage net client status.
    /// </summary>
    public class NetHandler : INetHandler
    {
        /// <summary>
        /// Client of handler.
        /// </summary>
        public INetClient Client { protected set; get; }

        /// <summary>
        /// On speed changed event.
        /// </summary>
        public event Action<double> OnSpeedChanged;

        /// <summary>
        /// On progress changed event.
        /// </summary>
        public event Action<float> OnProgressChanged;

        /// <summary>
        /// On completed event.
        /// </summary>
        public event Action<object, Exception> OnCompleted;

        /// <summary>
        /// Last speed value.
        /// </summary>
        protected double speed;

        /// <summary>
        /// Last progress value.
        /// </summary>
        protected float progress;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="client"></param>
        public NetHandler(INetClient client)
        {
            Client = client;
        }

        /// <summary>
        /// Notify status of client.
        /// </summary>
        public void NotifyStatus()
        {
            if (speed != Client.Speed)
            {
                speed = Client.Speed;
                OnSpeedChanged?.Invoke(speed);
            }

            if (progress != Client.Progress)
            {
                progress = Client.Progress;
                OnProgressChanged?.Invoke(progress);
            }

            if (Client.IsDone)
            {
                if (Client.Result != null || Client.Error != null)
                {
                    OnCompleted?.Invoke(Client.Result, Client.Error);
                }
            }
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        public void Dispose()
        {
            Client.Dispose();
            Client = null;

            OnSpeedChanged = null;
            OnProgressChanged = null;
            OnCompleted = null;
        }
    }
}