﻿/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  TestDownload.cs
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
    public class TestDownload : MonoBehaviour
    {
        public string url;
        INetHandler handler;

        void Start()
        {
            var file = string.Format("{0}/{1}", Application.dataPath, Path.GetFileName(url));
            handler = NetClientHubAPI.handler.DownloadAsync(url, 120000, file);

#if LISTEN_NOTIFY
            handler.OnProgressChanged += progress => Debug.Log($"progress: {progress.ToString("f3")}");
            handler.OnSpeedChanged += speed =>
            {
                speed /= 1024;
                var unit = "kb/s";
                if (speed >= 1024)
                {
                    speed /= 1024;
                    unit = "mb/s";
                }
                Debug.LogFormat("speed is {0} {1}", speed.ToString("f2"), unit);
            };
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
                var speed = handler.Client.Speed / 1024;
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