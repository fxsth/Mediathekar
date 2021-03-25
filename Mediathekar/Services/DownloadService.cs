using System;
using System.Collections.Generic;
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
            foreach(var file in Downloads)
            {
                if (file.Url == url)
                    return true;
            }
            return false;
        }
        public void addToDownloadQueue(DownloadFile file)
        {
            Downloads.Enqueue(file);
            TaskQueue.Enqueue(async () => {
                await file.download();
                Completed.Enqueue(Downloads.Dequeue());
                return;
            });
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