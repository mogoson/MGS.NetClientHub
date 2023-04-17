using MGS.Work.Net;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NetPostWorkTest
    {
        [Test]
        public void TestExecute()
        {
            var url = "https://www.baidu.com/";
            var postData = @"{""content"":""content""}";
            var headData = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var work = new NetPostWork(url, 60000, postData, headData);
            work.Execute();

            Assert.IsNull(work.Error);
            Debug.Log($"work.Result {work.Result}");
            Assert.IsNotNull(work.Result);
        }

        [UnityTest]
        public IEnumerator TestExecuteAsync()
        {
            var url = "https://www.baidu.com/";
            var postData = @"{""content"":""content""}";
            var headData = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var work = new NetPostWork(url, 60000, postData, headData);
            work.ExecuteAsync();

            while (!work.IsDone)
            {
                yield return null;
                Debug.Log($"Speed {work.Speed} byte/s");
                Debug.Log($"Progress {work.Progress}");
            }

            Assert.IsNull(work.Error);
            Debug.Log($"work.Result {work.Result}");
            Assert.IsNotNull(work.Result);
        }
    }
}