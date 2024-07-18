/*************************************************************************
 *  Copyright © 2022 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  NetFileClient.cs
 *  Description  :  Net file client.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/20/2022
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace MGS.Net
{
    /// <summary>
    /// Net file client.
    /// </summary>
    public class NetFileClient : NetClient<string>
    {
        /// <summary>
        /// Size(byte) of buffer to copy stream.
        /// </summary>
        protected const int BUFFER_SIZE = 1024 * 1024;

        /// <summary>
        /// Cycle(ms) of statistics.
        /// </summary>
        protected const int STATISTICS_CYCLE = 250;

        /// <summary>
        /// Path of local file.
        /// </summary>
        protected string filePath;

        /// <summary>
        /// Path of temp file.
        /// </summary>
        protected string tempFile;

        /// <summary>
        /// Size of temp file.
        /// </summary>
        protected long tempSize;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath">Path of local file.</param>
        /// <param name="headData">Head data of request.</param>
        public NetFileClient(string url, int timeout, string filePath, IDictionary<string, string> headData = null) : base(url, timeout, headData)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Do request work.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        protected override void DoRequest(HttpWebRequest request)
        {
            tempFile = GetTempFilePath(URL, filePath);
            tempSize = GetFileLength(tempFile);
            request.AddRange(tempSize);

            base.DoRequest(request);
        }

        /// <summary>
        /// Read Result from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected override string ReadResult(Stream stream)
        {
            Size += tempSize;
            RequireDirectory(tempFile);

            using (var fileStream = new FileStream(tempFile, FileMode.Append))
            {
                int readSize;
                var buffer = new byte[BUFFER_SIZE];

                float cacheSize = tempSize;
                var statisticsSize = 0f;
                var statisticsTimer = 0d;
                var lastStatisticsTicks = DateTime.Now.Ticks;

                //Not canceled and can read buffer.
                while (!IsDone && (readSize = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, readSize);

                    cacheSize += readSize;
                    Progress = cacheSize / Size;

                    statisticsSize += readSize;
                    statisticsTimer = (DateTime.Now.Ticks - lastStatisticsTicks) * 1e-4;
                    if (statisticsTimer >= STATISTICS_CYCLE)
                    {
                        Speed = statisticsSize / (statisticsTimer * 1e-3);
                        lastStatisticsTicks = DateTime.Now.Ticks;
                        statisticsSize = 0;
                    }
                }
                Speed = 0f;
            }

            //Not canceled.
            if (!IsDone)
            {
                RequireDirectory(filePath);
                File.Move(tempFile, filePath);
            }
            return filePath;
        }

        /// <summary>
        /// Get temp file for url and dest path.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected virtual string GetTempFilePath(string url, string filePath)
        {
            return string.Format("{0}/{1}.temp", Path.GetDirectoryName(filePath), url.GetHashCode());
        }

        /// <summary>
        /// Get length of local file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected long GetFileLength(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return 0;
            }

            return new FileInfo(filePath).Length;
        }

        /// <summary>
        /// Require Dir.
        /// </summary>
        /// <param name="path"></param>
        protected void RequireDirectory(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}