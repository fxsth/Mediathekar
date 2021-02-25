using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace MediaLibrarian.Models
{
    public class DownloadFile
    {
        public DownloadFile(string url, string filename, MediaType mediaType)
        {
            Url = url;
            Filename = filename.Trim(Path.GetInvalidFileNameChars()); ;
            MediaType = mediaType;

            download();
        }
        public DownloadFile(MediaElement element)
        {
            Url = element.Url;
            if (element.MediaType == MediaType.Series && element.Episode.HasValue)
            {
                Filename = "S" + element.Season + "E" + element.Episode + " " + element.Title;
            }
            else
            {
                string year ="";
                if(element.Year.HasValue)
                {
                    year = " (" + element.Year + ")";
                }
                Filename = element.Title+year;
            }
            Filename = string.Join("-", Filename.Split(Path.GetInvalidFileNameChars()));
            MediaType = element.MediaType;

            download();
        }

        public string Url { get; set; }
        public string Filename { get; private set; }
        public MediaType MediaType { get; set; }
        public int Progress { get; private set; }
        public string? Status { get; private set; }

        private IMediaInfo MediaInfo;
        private async Task<IConversionResult> download()
        {
            try
            {
                //FFmpeg.SetExecutablesPath("C:\\Program Files\\ffmpeg\\bin");
                string moviesDir;
                if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
                {
                    moviesDir = Environment.GetEnvironmentVariable("Movies");
                }
                else
                {
                    var pathWithEnv = @"%USERPROFILE%\Downloads";
                    moviesDir = Environment.ExpandEnvironmentVariables(pathWithEnv);
                }
                MediaInfo = await FFmpeg.GetMediaInfo(Url);

                string outputPath = Path.Combine(moviesDir, Filename + ".mp4");
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
                Status = e.Message;
                return null;
            }
        }

        private IVideoStream selectStreamWithHighestBitrate(IEnumerable<IVideoStream> streams)
        {
            return streams.ToList().OrderByDescending(stream => stream.Bitrate).First();
        }
        private IAudioStream selectStreamWithHighestBitrate(IEnumerable<IAudioStream> streams)
        {
            return streams.ToList().OrderByDescending(stream => stream.Bitrate).First();
        }
    }

    public enum MediaType
    {
        Movie,
        Series
    }
}
