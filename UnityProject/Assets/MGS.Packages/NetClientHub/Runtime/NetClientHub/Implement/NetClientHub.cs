/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetClientHub.cs
 *  Description  :  Hub to manage net clients.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using System.Threading;

namespace MGS.Net
{
    /// <summary>
    /// Hub to manage net clients.
    /// </summary>
    public class NetClientHub : INetClientHub
    {
        /// <summary>
        /// Max count of concurrency clients.
        /// </summary>
        public int Concurrency { set; get; }

        /// <summary>
        /// Net resolver to check retrieable.
        /// </summary>
        public INetResolver Resolver { set; get; }

        /// <summary>
        /// Queue for waiting clients.
        /// </summary>
        protected Queue<INetClient> waitingClients = new Queue<INetClient>();

        /// <summary>
        /// List for working clients.
        /// </summary>
        protected List<INetClient> workingClients = new List<INetClient>();

        /// <summary>
        /// Cycle(ms) for one tick.
        /// </summary>
        private const int TICK_CYCLE = 250;

        /// <summary>
        /// Mark is disposed?
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="concurrency">Max count of concurrency clients.</param>
        /// <param name="resolver">Net resolver to check retrieable.</param>
        public NetClientHub(int concurrency = 3, INetResolver resolver = null)
        {
            Concurrency = concurrency;
            Resolver = resolver;
            new Thread(Tick) { IsBackground = true }.Start();
        }

        /// <summary>
        /// Put url to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <returns></returns>
        public virtual INetClient Put(string url, int timeout)
        {
            var client = new NetPutClient(url, timeout, null) { DontSetDoneIfError = true };
            waitingClients.Enqueue(client);
            return client;
        }

        /// <summary>
        /// Post url and data to server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public virtual INetClient Post(string url, int timeout, string postData)
        {
            var client = new NetPostClient(url, timeout, null, postData) { DontSetDoneIfError = true };
            waitingClients.Enqueue(client);
            return client;
        }

        /// <summary>
        /// Download file from server.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public virtual INetClient Download(string url, int timeout, string filePath)
        {
            var client = new NetFileClient(url, timeout, filePath) { DontSetDoneIfError = true };
            waitingClients.Enqueue(client);
            return client;
        }

        /// <summary>
        /// Discard net clients.
        /// </summary>
        /// <param name="workings">Discard the working clients?</param>
        /// <param name="waitings">Discard the waiting clients?</param>
        public virtual void Discard(bool workings, bool waitings)
        {
            if (workings)
            {
                foreach (var client in workingClients)
                {
                    client.Close();
                }
                workingClients.Clear();
            }
            if (waitings)
            {
                waitingClients.Clear();
            }
        }

        /// <summary>
        /// Dispose all resource.
        /// </summary>
        public virtual void Dispose()
        {
            Discard(true, true);

            if (Resolver != null)
            {
                Resolver.Dispose();
                Resolver = null;
            }

            workingClients = null;
            waitingClients = null;
            isDisposed = true;
        }

        /// <summary>
        /// Tick loop to update.
        /// </summary>
        private void Tick()
        {
            while (!isDisposed)
            {
                TickUpdate();
                Thread.Sleep(TICK_CYCLE);
            }
        }

        /// <summary>
        /// Update to dispatch clients.
        /// </summary>
        protected virtual void TickUpdate()
        {
            // Dequeue waitings to workings.
            while (waitingClients.Count > 0 && workingClients.Count < Concurrency)
            {
                var client = waitingClients.Dequeue();
                client.Open();
                workingClients.Add(client);
            }

            // Check workings.
            for (int i = 0; i < workingClients.Count; i++)
            {
                var client = workingClients[i];
                if (client.IsDone)
                {
                    workingClients.RemoveAt(i);
                    ClearResolver(client);
                    OnClientIsDone(client);
                    i--;
                }
                else
                {
                    if (client.Error != null)
                    {
                        if (CheckRetrieable(client))
                        {
                            client.Open();
                        }
                        else
                        {
                            if (client is NetClient)
                            {
                                (client as NetClient).DontSetDoneIfError = false;
                            }
                            client.Close();

                            workingClients.RemoveAt(i);
                            ClearResolver(client);
                            OnClientIsError(client);
                            i--;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// On client is done.
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnClientIsDone(INetClient client) { }

        /// <summary>
        /// On client is error.
        /// </summary>
        /// <param name="client"></param>
        protected virtual void OnClientIsError(INetClient client) { }

        /// <summary>
        /// Check client is retrieable?
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected bool CheckRetrieable(INetClient client)
        {
            if (Resolver == null)
            {
                return false;
            }
            return Resolver.Retrieable(client);
        }

        /// <summary>
        /// Clear the history of client in resolver.
        /// </summary>
        /// <param name="client"></param>
        protected void ClearResolver(INetClient client)
        {
            if (Resolver != null)
            {
                Resolver.Clear(client);
            }
        }
    }
}