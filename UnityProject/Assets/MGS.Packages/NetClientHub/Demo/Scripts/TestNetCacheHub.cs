/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  TestNetCacheHub.cs
 *  Description  :  Ignore.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using UnityEngine;

namespace MGS.Net.Demo
{
    public class TestNetCacheHub : MonoBehaviour
    {
        public string url;
        INetClient client;
        INetClientHub hub;

        void Start()
        {
            var cacher = new Cacher<string>(100, 10000);
            var resolver = new NetResolver(3, new Type[] { typeof(TimeoutException) });
            hub = new NetCacheHub(cacher, 3, resolver);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (client == null)
                {
                    client = hub.Put(url, 1000);
                    if (client is CacheClient)
                    {
                        Debug.LogFormat("Respond from cache.");
                    }
                    else
                    {
                        Debug.LogFormat("Request to server.");
                    }
                }
            }

            if (client != null)
            {
                if (!client.IsDone)
                {
                    Debug.LogFormat("progress is {0}", client.Progress);
                    Debug.LogFormat("Speed is {0} kb", client.Speed / 1024);
                }
                else
                {
                    if (client.Error == null)
                    {
                        Debug.LogFormat("result is {0}", client.Result);
                    }
                    else
                    {
                        Debug.LogFormat("error is {0}", client.Error.Message);
                    }

                    client.Close();
                    client = null;
                }
            }
        }

        private void OnDestroy()
        {
            hub.Dispose();
            client = null;
        }
    }
}