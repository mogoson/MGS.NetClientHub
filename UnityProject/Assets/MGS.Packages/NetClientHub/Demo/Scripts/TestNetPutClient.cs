/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  TestNetPutClient.cs
 *  Description  :  Ignore.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEngine;

namespace MGS.Net.Demo
{
    public class TestNetPutClient : MonoBehaviour
    {
        public string url;
        INetClient client;

        void Start()
        {
            client = new NetPutClient(url, 120000, null);
            client.Open();
        }

        void Update()
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
                    Debug.LogErrorFormat("error is {0}", client.Error.Message);
                }

                client.Close();
                client = null;
                enabled = false;
            }
        }

        void OnDestroy()
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }
        }
    }
}