/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetResolver.cs
 *  Description  :  Net resolver to check retrieable.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;

namespace MGS.Net
{
    /// <summary>
    /// Net resolver to check retrieable.
    /// </summary>
    public class NetResolver : INetResolver
    {
        /// <summary>
        /// Retry times.
        /// </summary>
        protected int times;

        /// <summary>
        /// Tolerable exception types can be retry.
        /// </summary>
        protected ICollection<Type> tolerables;

        /// <summary>
        /// Tolerance times.
        /// </summary>
        protected Dictionary<string, int> toleranceTimes;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="times">Retry times.</param>
        /// <param name="tolerables">Tolerable exception types can be retry.</param>
        public NetResolver(int times, ICollection<Type> tolerables)
        {
            this.times = times;
            this.tolerables = tolerables;
            toleranceTimes = new Dictionary<string, int>();
        }

        /// <summary>
        /// Check client is retrieable?
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool Retrieable(INetClient client)
        {
            if (tolerables == null || !tolerables.Contains(client.Error.GetType()))
            {
                return false;
            }

            var tts = 0;
            if (toleranceTimes.ContainsKey(client.Key))
            {
                tts = toleranceTimes[client.Key];
            }

            if (tts < times)
            {
                toleranceTimes[client.Key] = tts + 1;
                return true;
            }
            else
            {
                Clear(client);
                return false;
            }
        }

        /// <summary>
        /// Clear the history of client.
        /// </summary>
        /// <param name="client"></param>
        public void Clear(INetClient client)
        {
            toleranceTimes.Remove(client.Key);
        }

        /// <summary>
        /// Clear the history of all clients.
        /// </summary>
        public void Clear()
        {
            toleranceTimes.Clear();
        }

        /// <summary>
        /// Dispose all.
        /// </summary>
        public void Dispose()
        {
            tolerables = null;
            toleranceTimes = null;
        }
    }
}