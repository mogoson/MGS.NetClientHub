using MGS.Work;
using MGS.Work.Net;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NetWorkHubTest
    {
        INetWorkHub hub;
        IAsyncWorkHandler<string> handler;

        [SetUp]
        public void SetUp()
        {
            hub = NetWorkHubFatory.CreateHub();
        }

        [TearDown]
        public void TearDown()
        {
            handler.Abort();
            handler = null;

            hub.Abort();
            hub = null;
        }

        [UnityTest]
        public IEnumerator GetAsyncTest()
        {
            var url = "https://www.baidu.com/";
            handler = hub.GetAsync(url, 60000);
            handler.OnProgressChanged += progress =>
            {
                Debug.Log($"Progress {progress}");
            };
            handler.OnSpeedChanged += speed =>
            {
                Debug.Log($"Speed {speed} byte/s");
            };
            handler.OnCompleted += (r, e) =>
            {
                if (e == null)
                {
                    Debug.Log($"Result {r}");
                }
                else
                {
                    Debug.Log($"Error {e.Message}/{e.StackTrace}");
                }
            };

            yield return handler.WaitDone();

            Assert.IsNull(handler.Work.Error);
            Debug.Log($"work.Result {handler.Work.Result}");
            Assert.IsNotNull(handler.Work.Result);
        }

        [UnityTest]
        public IEnumerator PostAsyncTest()
        {
            var url = "https://www.baidu.com/";
            var postData = @"{""content"":""content""}";
            var headData = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            handler = hub.PostAsync(url, 60000, postData, headData);
            handler.OnProgressChanged += progress =>
            {
                Debug.Log($"Progress {progress}");
            };
            handler.OnSpeedChanged += speed =>
            {
                Debug.Log($"Speed {speed} byte/s");
            };
            handler.OnCompleted += (r, e) =>
            {
                if (e == null)
                {
                    Debug.Log($"Result {r}");
                }
                else
                {
                    Debug.Log($"Error {e.Message}/{e.StackTrace}");
                }
            };

            yield return handler.WaitDone();

            Assert.IsNull(handler.Work.Error);
            Debug.Log($"work.Result {handler.Work.Result}");
            Assert.IsNotNull(handler.Work.Result);
        }

        [UnityTest]
        public IEnumerator DownloadAsyncTest()
        {
            var url = "https://www.baidu.com/s?wd=%E7%99%BE%E5%BA%A6%E7%83%AD%E6%90%9C&sa=ire_dl_gh_logo_texing&rsv_dl=igh_logo_pcshttps://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png";
            var filePath = $"{Application.dataPath}/../Test/{Path.GetFileName(url)}";
            handler = hub.DownloadAsync(url, 60000, filePath);
            handler.OnProgressChanged += progress =>
            {
                Debug.Log($"Progress {progress}");
            };
            handler.OnSpeedChanged += speed =>
            {
                Debug.Log($"Speed {speed} byte/s");
            };
            handler.OnCompleted += (r, e) =>
            {
                if (e == null)
                {
                    Debug.Log($"Result {r}");
                }
                else
                {
                    Debug.Log($"Error {e.Message}/{e.StackTrace}");
                }
            };

            yield return handler.WaitDone();

            Assert.IsNull(handler.Work.Error);
            var file = handler.Work.Result;
            Debug.Log($"file {file}");
            Assert.IsTrue(File.Exists(file));
        }
    }
}