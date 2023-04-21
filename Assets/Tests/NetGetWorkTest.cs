using MGS.Work.Net;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NetGetWorkTest
    {
        [Test]
        public void TestExecute()
        {
            var url = "https://www.baidu.com/";
            var work = new NetGetWork(url, 60000);
            work.Execute();

            Assert.IsNull(work.Error);
            Debug.Log($"work.Result {work.Result}");
            Assert.IsNotNull(work.Result);
        }

        [UnityTest]
        public IEnumerator TestExecuteAsync()
        {
            var url = "https://www.baidu.com/";
            var work = new NetGetWork(url, 60000);
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