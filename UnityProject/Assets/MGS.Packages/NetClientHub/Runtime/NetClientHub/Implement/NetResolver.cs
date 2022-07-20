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
        protected Dictionary<int, int> toleranceTimes;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="times">Retry times.</param>
        /// <param name="tolerables">Tolerable exception types can be retry.</param>
        public NetResolver(int times, ICollection<Type> tolerables)
        {
            this.times = times;
            this.tolerables = tolerables;
            toleranceTimes = new Dictionary<int, int>();
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
            var key = client.URL.GetHashCode();
            if (toleranceTimes.ContainsKey(key))
            {
                tts = toleranceTimes[key];
            }

            if (tts < times)
            {
                toleranceTimes[key] = tts + 1;
                return true;
            }
            else
            {
                Clear(key);
                return false;
            }
        }

        /// <summary>
        /// Clear the history of client.
        /// </summary>
        /// <param name="client"></param>
        public void Clear(INetClient client)
        {
            Clear(client.URL.GetHashCode());
        }

        /// <summary>
        /// Dispose all.
        /// </summary>
        public void Dispose()
        {
            tolerables = null;
            toleranceTimes = null;
        }

        /// <summary>
        /// Clear the history by key.
        /// </summary>
        /// <param name="client"></param>
        protected void Clear(int key)
        {
            toleranceTimes.Remove(key);
        }
    }
}