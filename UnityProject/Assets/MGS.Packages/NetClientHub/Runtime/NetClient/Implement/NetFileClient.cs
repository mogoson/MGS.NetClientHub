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
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace MGS.Net
{
    /// <summary>
    /// Net file client.
    /// </summary>
    public class NetFileClient : NetClient
    {
        /// <summary>
        /// Size(byte) of buffer to copy stream.
        /// </summary>
        protected const int BUFFER_SIZE = 1024 * 1024;

        /// <summary>
        /// Cycle(ms) of statistics.
        /// </summary>
        protected const int STATISTICS_CYCLE = 500;

        /// <summary>
        /// Path of local file.
        /// </summary>
        public string FilePath { protected set; get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">Remote url string.</param>
        /// <param name="timeout">Timeout(ms) of request.</param>
        /// <param name="filePath">Path of local file.</param>
        /// <param name="headData">Head data of request.</param>
        public NetFileClient(string url, int timeout, string filePath, IDictionary<string, string> headData = null) : base(url, timeout, headData)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Do request work.
        /// </summary>
        /// <param name="webClient"></param>
        /// <param name="url"></param>
        protected override void DoRequest(WebClientEx webClient, string url)
        {
            var tempFile = GetTempFilePath(url, FilePath);
            var tempSize = GetFileLength(tempFile);
            webClient.Range = tempSize;
            webClient.OpenReadCompleted += (s, e) => WebClient_OpenReadCompleted(s, e, tempFile, tempSize, FilePath);
            webClient.OpenReadAsync(new Uri(url));
        }

        /// <summary>
        /// OpenReadCompleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="tempFile"></param>
        /// <param name="tempSize"></param>
        /// <param name="destFile"></param>
        protected void WebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e, string tempFile, long tempSize, string destFile)
        {
            if (!e.Cancelled && e.Error == null)
            {
                var stream = e.Result;
                Size = tempSize + stream.Length;

                var encoding = (sender as WebClientEx).ResponseHeaders["Content-Encoding"];
                if (encoding == "gzip")
                {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                }
                var streamCopyThread = new Thread(() => CopyStreamToFile(stream, tempFile, tempSize, destFile)) { IsBackground = true };
                streamCopyThread.Start();
            }
            else
            {
                Error = e.Error;
                Close();
            }
        }

        /// <summary>
        /// Copy data from source stream to local file stream.
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="tempFile"></param>
        /// <param name="tempSize"></param>
        /// <param name="destFile"></param>
        protected void CopyStreamToFile(Stream sourceStream, string tempFile, long tempSize, string destFile)
        {
            try
            {
                RequireDirectory(tempFile);
                using (var fileStream = new FileStream(tempFile, FileMode.Append))
                {
                    int readSize;
                    var buffer = new byte[BUFFER_SIZE];

                    float cacheSize = tempSize;
                    var statisticsSize = 0f;
                    var statisticsTimer = 0d;
                    var lastStatisticsTicks = DateTime.Now.Ticks;

                    while (!IsDone && (readSize = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
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
                }

                if (!IsDone)
                {
                    RequireDirectory(destFile);
                    File.Move(tempFile, destFile);
                    Speed = 0f;
                    Progress = 1.0f;
                    Result = destFile;
                }
            }
            catch (Exception ex)
            {
                Error = ex;
            }
            finally
            {
                sourceStream.Close();
                Close();
            }
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