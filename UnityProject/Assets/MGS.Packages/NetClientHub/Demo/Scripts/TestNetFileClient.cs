/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  TestNetFileClient.cs
 *  Description  :  Ignore.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System.IO;
using UnityEngine;

namespace MGS.Net.Demo
{
    public class TestNetFileClient : MonoBehaviour
    {
        public string url;
        INetClient client;

        void Start()
        {
            var file = string.Format("{0}/{1}", Application.dataPath, Path.GetFileName(url));
            client = NetClientHubAPI.handler.Download(url, 120000, file);
        }

        void Update()
        {
            if (!client.IsDone)
            {
                Debug.LogFormat("progress is {0}", client.Progress.ToString("f3"));
                var speed = client.Speed / 1024;
                var unit = "kb/s";
                if (speed >= 1024)
                {
                    speed /= 1024;
                    unit = "mb/s";
                }

                Debug.LogFormat("Speed is {0} {1}", speed.ToString("f2"), unit);
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