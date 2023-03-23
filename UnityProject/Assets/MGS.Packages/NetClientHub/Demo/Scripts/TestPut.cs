/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  TestPut.cs
 *  Description  :  Ignore.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

#define LISTEN_NOTIFY

using System.IO;
using UnityEngine;

namespace MGS.Net.Demo
{
    public class TestPut : MonoBehaviour
    {
        public string url;
        INetHandler handler;

        void Start()
        {
            handler = NetClientHubAPI.handler.PutAsync(url, 120000);

#if LISTEN_NOTIFY
            handler.OnProgressChanged += progress => Debug.Log($"progress: {progress.ToString("f3")}");
            handler.OnCompleted += (result, error) =>
            {
                var msg = error == null ? "" : error.Message;
                Debug.Log($"result: {result}, error: {msg}");
            };
#endif
        }

#if !LISTEN_NOTIFY
        void Update()
        {
            if (!handler.Client.IsDone)
            {
                Debug.LogFormat("progress is {0}", handler.Client.Progress.ToString("f3"));
            }
            else
            {
                if (handler.Client.Error == null)
                {
                    Debug.LogFormat("result is {0}", handler.Client.Result);
                }
                else
                {
                    Debug.LogErrorFormat("error is {0}", handler.Client.Error.Message);
                }
                enabled = false;
            }
        }
#endif
        void OnDestroy()
        {
            handler.Dispose();
        }
    }
}