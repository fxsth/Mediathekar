using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mediathekar.Models
{
    public class DownloadFile
    {
        public DownloadFile() { }
        public DownloadFile(string url, string filename, MediaType mediaType)
        {
            Url = url;
            Filename = filename.Trim(Path.GetInvalidFileNameChars()); ;
            MediaType = mediaType;
        }
        public DownloadFile(MediaElement element)
        {
            _mediaElement = element;
            Url = element.Url;
            string subdir = removeIllegalChars(element.Topic);
            if (element.MediaType == MediaType.Series)
            {
                Filename = "";
                if (element.Episode.HasValue)
                    Filename += "S" + element.Season;
                if (element.Episode.HasValue)
                    Filename += "E" + element.Episode;
                Filename += " " + element.Title;
                Filename = removeIllegalChars(Filename);
                Filename = Path.Combine(subdir, Filename);
            }
            else
            {
                string year = "";
                if (element.Year.HasValue)
                {
                    year = " (" + element.Year + ")";
                }
                Filename = element.Title + year;
                Filename = removeIllegalChars(Filename);
            }
            MediaType = element.MediaType;
        }
        private MediaElement _mediaElement { get; set; }

        public string Url { get; set; }
        public string Filename { get; private set; }
        public MediaType MediaType { get; set; }
        public int Progress { get; private set; }
        public string? Status { get; set; }
        private string removeIllegalChars(string str)
        {
            str = string.Join("-", str.Split(Path.GetInvalidFileNameChars()));
            str = string.Join("-", str.Split(Path.GetInvalidPathChars()));
            str = str.Replace(",", "").Replace("!", "").Replace(":", "").Replace(";", "");
            str = str.Trim();
            return str;
        }
    }
}
