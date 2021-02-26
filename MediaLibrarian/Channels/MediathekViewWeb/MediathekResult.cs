using System;
using System.Collections.Generic;

namespace ResultData
{
    public class MediaElement
    {
        public string channel { get; set; }
        public string topic { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime timestamp { get; set; }
        public uint? duration { get; set; }
        public uint? size { get; set; }
        public string url_website { get; set; }
        public string url_subtitle { get; set; }
        public string url_video { get; set; }
        public string url_video_low { get; set; }
        public string url_video_hd { get; set; }
        public string filmlisteTimestamp { get; set; }
        public string id { get; set; }
    }

    public class QueryInfo
    {
        public string filmlisteTimestamp { get; set; }
        public string searchEngineTime { get; set; }
        public int resultCount { get; set; }
        public int totalResults { get; set; }
    }
    public class Result
    {
        public List<MediaElement> results { get; set; }
        public QueryInfo queryInfo { get; set; }
    }

    ///<summary>root class for json response</summary>
    public class MediathekResult
    {
        public Result result { get; set; }
        public object err { get; set; }

        public int countResults()
        {
            if (result?.results == null)
            {
                return 0;
            }
            return result.results.Count;
        }
        public bool hasErrors()
        {
            return err != null;
        }

        public static List<MediaLibrarian.Models.MediaElement> ToMediaElements(MediathekResult mediathekResult)
        {
            if(mediathekResult.hasErrors())
            {
                return null;
            }
            List<MediaLibrarian.Models.MediaElement> mediaElements = new List<MediaLibrarian.Models.MediaElement>();
            foreach (var medium in mediathekResult.result.results)
            {
                var element = new MediaLibrarian.Models.MediaElement
                {
                    Channel = medium.channel,
                    IdInChannel = "MediathekViewWeb" + medium.id,       // reuse id, add channel for uniqueness
                    LastModified = medium.timestamp,
                    Title = medium.title,
                    Topic = medium.topic,
                    Url = medium.url_video_hd,
                    MediaType = MediaLibrarian.Models.MediaType.Movie   // standard
                };
                int? season = null;
                int? episode = null;
                
                if (MediaLibrarian.Models.Utilities.MediaElementUtilities.ContainsSeasonEpisode(medium.title, ref season, ref episode))
                {
                    element.Season = season;
                    element.Episode = episode;
                    element.MediaType = MediaLibrarian.Models.MediaType.Series;
                }
                if(medium.duration < 45 * 60)
                {
                    // less than 45min -> not a movie
                    element.MediaType = MediaLibrarian.Models.MediaType.Series;
                }
                mediaElements.Add(element);
            }
            return mediaElements;
        }
    }
}