using System.Collections.Generic;
using MediaLibrarian.Models;

namespace MediaLibrarian.Services
{
    public class DownloadService
    {

        public List<DownloadFile> Downloads;

        public DownloadService(List<DownloadFile> downloads)
        {
            Downloads = downloads;
        }

        public DownloadService()
        {
            Downloads = new List<DownloadFile>();
        }
    }
}