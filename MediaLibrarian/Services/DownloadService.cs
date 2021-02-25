using System.Collections.Generic;
using MediaLibrarian.Models;

namespace MediaLibrarian.Services
{
    public class DownloadService
    {
        public Queue<DownloadFile> Downloads;

        public DownloadService(Queue<DownloadFile> downloads)
        {
            Downloads = downloads;
        }

        public DownloadService()
        {
            Downloads = new Queue<DownloadFile>();
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
            file.startDownload();
        }
    }
}