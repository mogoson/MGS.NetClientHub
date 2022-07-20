/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Cacher.cs
 *  Description  :  Cacher for data.
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
    /// Cacher for data.
    /// </summary>
    /// <typeparam name="T">Type of cache data.</typeparam>
    public class Cacher<T> : ICacher<T>
    {
        /// <summary>
        /// Max count of caches.
        /// </summary>
        public int MaxCache { set; get; }

        /// <summary>
        /// Timeout(ms).
        /// </summary>
        public int Timeout { set; get; }

        /// <summary>
        /// Cache stocks.
        /// </summary>
        protected Dictionary<string, Stock> stocks = new Dictionary<string, Stock>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxCache">Max count of caches.</param>
        /// <param name="timeout">Timeout(ms).</param>
        public Cacher(int maxCache = 100, int timeout = 1000)
        {
            MaxCache = maxCache;
            Timeout = timeout;
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, T content)
        {
            var keys = stocks.Keys.GetEnumerator();
            while (stocks.Count > MaxCache)
            {
                keys.MoveNext();
                stocks.Remove(keys.Current);
            }

            var info = new Stock(content);
            stocks[key] = info;
        }

        /// <summary>
        /// Get cache data.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get(string key)
        {
            if (stocks.ContainsKey(key))
            {
                var info = stocks[key];
                var ms = (DateTime.Now - info.stamp).TotalMilliseconds;
                if (ms > Timeout)
                {
                    Remove(key);
                    return default(T);
                }
                return info.content;
            }
            return default(T);
        }

        /// <summary>
        /// Remove cache data.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            stocks.Remove(key);
        }

        /// <summary>
        /// Clear all caches.
        /// </summary>
        public void Clear()
        {
            stocks.Clear();
        }

        /// <summary>
        /// Dispose all resources.
        /// </summary>
        public void Dispose()
        {
            Clear();
            stocks = null;
        }

        /// <summary>
        /// Stock cache.
        /// </summary>
        protected struct Stock
        {
            /// <summary>
            /// Content of stock.
            /// </summary>
            public T content;

            /// <summary>
            /// Stamp of stock.
            /// </summary>
            public DateTime stamp;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="content">Content of stock.</param>
            public Stock(T content)
            {
                this.content = content;
                stamp = DateTime.Now;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="content">Content of stock.</param>
            /// <param name="stamp">Stamp of stock.</param>
            public Stock(T content, DateTime stamp)
            {
                this.content = content;
                this.stamp = stamp;
            }
        }
    }
}