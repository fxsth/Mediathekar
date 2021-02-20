using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaLibrarian.Models
{
    public class DownloadFile
    {
        public string Url { get; set; }
        public MediaType MediaType { get; set; }
    }

    public enum MediaType
    {
        Movie,
        Series
    }
}
