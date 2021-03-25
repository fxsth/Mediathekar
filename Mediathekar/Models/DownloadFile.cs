using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Exceptions;

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
            if (element.MediaType == MediaType.Series && element.Episode.HasValue)
            {
                Filename = "S" + element.Season + "E" + element.Episode + " " + element.Title;
                Filename = removeIllegalChars(Filename);
                //Filename = Path.Combine(subdir, Filename);
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
        public string? Status { get; private set; }

        private IMediaInfo MediaInfo;

        public async Task<IConversionResult> download()
        {
            try
            {
                string outputPath = GetOutputPath();
                if (File.Exists(outputPath))
                {
                    Status = "File exists already";
                    return null;
                }
                Console.WriteLine("GetMediaInfo called with: " + Url);
                MediaInfo = await FFmpeg.GetMediaInfo(Url);
                Console.WriteLine("Got the MediaInfo - FFmpeg seems to work fine");
                var videoStream = selectStreamWithHighestBitrate(MediaInfo.VideoStreams).CopyStream();
                var audioStream = selectStreamWithHighestBitrate(MediaInfo.AudioStreams).CopyStream();
                IConversion conversion = FFmpeg.Conversions.New();
                conversion.AddStream(videoStream).AddStream(audioStream).SetOutput(outputPath);
                conversion.OnProgress += (sender, args) =>
                {
                    var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                    Debug.WriteLine($"[{args.Duration} / {args.TotalLength}] {percent}%");
                    Progress = percent;
                };
                IConversionResult result = await conversion.Start();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Downlod-Exception: " + e.Message);
                Status = e.Message;
                if (e.InnerException != null)
                    Console.WriteLine(e.InnerException.Message);
                return null;
            }
        }
        private string GetOutputPath()
        {
            Console.WriteLine("Starting GetOutputPath()");
            string baseDir;
            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                // Docker
                if (MediaType == MediaType.Movie)
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
            if (baseDir.Length == 0 || !Directory.Exists(baseDir))
                baseDir = Directory.GetCurrentDirectory();
            Console.WriteLine("BaseDir is: " + baseDir);
            Console.WriteLine("Filename is: " + Filename);
            return Path.Combine(baseDir, Filename + ".mp4");
        }
        private string removeIllegalChars(string str)
        {
            str = string.Join("-", str.Split(Path.GetInvalidFileNameChars()));
            str = string.Join("-", str.Split(Path.GetInvalidPathChars()));
            str = str.Replace(",", "").Replace("!", "").Replace(":", "").Replace(";", "");
            str = str.Trim();
            return str;
        }
        private static IVideoStream selectStreamWithHighestBitrate(IEnumerable<IVideoStream> streams)
        {
            return streams.ToList().OrderByDescending(stream => stream.Bitrate).First();
        }
        private static IAudioStream selectStreamWithHighestBitrate(IEnumerable<IAudioStream> streams)
        {
            return streams.ToList().OrderByDescending(stream => stream.Bitrate).First();
        }
    }
}
