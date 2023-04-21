using MGS.Work.Net;
using NUnit.Framework;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NetFileWorkTest
    {
        [Test]
        public void TestExecute()
        {
            var url = "https://www.baidu.com/s?wd=%E7%99%BE%E5%BA%A6%E7%83%AD%E6%90%9C&sa=ire_dl_gh_logo_texing&rsv_dl=igh_logo_pcshttps://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png";
            var filePath = $"{Application.dataPath}/../Test/{Path.GetFileName(url)}";
            var work = new NetFileWork(url, 60000, filePath);
            work.Execute();

            Assert.IsNull(work.Error);
            var file = work.Result;
            Debug.Log($"file {file}");
            Assert.IsTrue(File.Exists(file));
        }

        [UnityTest]
        public IEnumerator TestExecuteAsync()
        {
            var url = "https://www.baidu.com/s?wd=%E7%99%BE%E5%BA%A6%E7%83%AD%E6%90%9C&sa=ire_dl_gh_logo_texing&rsv_dl=igh_logo_pcshttps://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png";
            var filePath = $"{Application.dataPath}/../Test/{Path.GetFileName(url)}";
            var work = new NetFileWork(url, 60000, filePath);
            work.ExecuteAsync();

            while (!work.IsDone)
            {
                yield return null;
                Debug.Log($"Speed {work.Speed} byte/s");
                Debug.Log($"Progress {work.Progress}");
            }

            Assert.IsNull(work.Error);
            var file = work.Result;
            Debug.Log($"file {file}");
            Assert.IsTrue(File.Exists(file));
        }
    }
}