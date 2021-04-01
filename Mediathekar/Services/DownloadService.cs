using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mediathekar.Models;

namespace Mediathekar.Services
{
    public class DownloadService
    {
        public Queue<DownloadFile> Downloads;
        public Queue<DownloadFile> Completed;
        private TaskQueue TaskQueue;

        //public DownloadService(Queue<DownloadFile> downloads)
        //{
        //    Downloads = downloads;
        //    TaskQueue = new TaskQueue();
        //}

        public DownloadService()
        {
            Downloads = new Queue<DownloadFile>();
            Completed = new Queue<DownloadFile>();
            TaskQueue = new TaskQueue();
        }
        public bool existsAlready(DownloadFile file)
        {
            return Downloads.Contains(file);
        }
        public bool existsAlready(string url)
        {
            foreach (var file in Downloads)
            {
                if (file.Url == url)
                    return true;
            }
            return false;
        }
        public void addToDownloadQueue(DownloadFile file)
        {
            file.Status = "Waiting for Download";
            Downloads.Enqueue(file);
            TaskQueue.Enqueue(async () =>
            {
                await ExecuteFFmpegAsync(ref file).ConfigureAwait(false);
                file.Status = file.Status == "Downloading..." ? null : file.Status;
                Completed.Enqueue(Downloads.Dequeue());
                return;
            });
        }

        private static Task ExecuteFFmpegAsync(ref DownloadFile file)
        {
            var process = new Process
            {
                StartInfo = {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "ffmpeg",
                    Arguments = "-i " + file.Url + " -c copy \"" + GetOutputPath(file) + "\""
                }
            };
            process.Exited += (sender, args) =>
            {
                process.Dispose();
            };
            // exists already? Cancel!
            if (File.Exists(GetOutputPath(file)))
            {
                file.Status = "File exists already";
                return Task.CompletedTask;
            }
            else
                process.Start();
            file.Status = "Downloading...";
            return process.WaitForExitAsync();
        }


        private static string GetOutputPath(DownloadFile downloadFile)
        {
            Console.WriteLine("Starting GetOutputPath()");
            string baseDir;
            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                // Docker
                if (downloadFile.MediaType == MediaType.Movie)
                    baseDir = Environment.ExpandEnvironmentVariables(@"/movies");
                else
                    baseDir = Environment.ExpandEnvironmentVariables(@"/tv");
            }
            else
            {
                // Windows
                var pathWithEnv = @"%USERPROFILE%\Downloads";
                baseDir = Environment.ExpandEnvironmentVariables(pathWithEnv);
            }
            Console.WriteLine("BaseDir is: " + baseDir);
            Console.WriteLine("Filename is: " + downloadFile.Filename);
            string filePath = Path.Combine(baseDir, downloadFile.Filename + ".mp4");
            string subdir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(subdir))
                Directory.CreateDirectory(subdir);
            return filePath;
        }
    }

    public class TaskQueue
    {
        private SemaphoreSlim semaphore;
        public TaskQueue()
        {
            semaphore = new SemaphoreSlim(1);
        }

        public async Task<T> Enqueue<T>(Func<Task<T>> taskGenerator)
        {
            await semaphore.WaitAsync();
            try
            {
                return await taskGenerator();
            }
            finally
            {
                semaphore.Release();
            }
        }
        public async Task Enqueue(Func<Task> taskGenerator)
        {
            await semaphore.WaitAsync();
            try
            {
                await taskGenerator();
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}