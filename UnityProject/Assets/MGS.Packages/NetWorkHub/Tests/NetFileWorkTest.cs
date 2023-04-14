using MGS.Work.Net;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Windows;

namespace Tests
{
    public class NetFileWorkTest
    {
        [Test]
        public void TestExecute()
        {
            var url = "";
            var filePath = "";
            var work = new NetFileWork(url, 60000, filePath);
            work.Execute();

            var file = work.Result;
            Debug.Log($"file {file}");
            Assert.IsTrue(File.Exists(file));
        }

        [UnityTest]
        public IEnumerator TestExecuteAsync()
        {
            var url = "";
            var filePath = "";
            var work = new NetFileWork(url, 60000, filePath);
            work.ExecuteAsync();

            while (!work.IsDone)
            {
                yield return null;
                Debug.Log($"Speed {work.Speed} byte/s");
                Debug.Log($"Progress {work.Progress}");
            }

            var file = work.Result;
            Debug.Log($"file {file}");
            Assert.IsTrue(File.Exists(file));
        }
    }
}