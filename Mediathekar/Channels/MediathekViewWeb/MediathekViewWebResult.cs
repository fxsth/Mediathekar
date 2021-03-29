using System;
using System.Collections.Generic;

namespace Mediathekar.Channels.MediathekViewWeb
{
    public class MediaElement
    {
        public string channel { get; set; }
        public string topic { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime? timestamp { get; set; }
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
        public DateTime? filmlisteTimestamp { get; set; }
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
    public class MediathekViewWebResult
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

        public static List<Mediathekar.Models.MediaElement> ToMediaElements(MediathekViewWebResult mediathekResult)
        {
            if(mediathekResult.hasErrors())
            {
                return null;
            }
            List<Mediathekar.Models.MediaElement> mediaElements = new List<Mediathekar.Models.MediaElement>();
            foreach (var medium in mediathekResult.result.results)
            {
                string url = medium.url_video_hd;
                if (String.IsNullOrEmpty(url))
                {
                    url = medium.url_video;
                    if (String.IsNullOrEmpty(url))
                    {
                        url = medium.url_video_low;
                        if (String.IsNullOrEmpty(url))
                            continue;
                    }
                }
                var element = new Mediathekar.Models.MediaElement
                {
                    Channel = medium.channel,
                    IdInChannel = "MediathekViewWeb" + medium.id,       // reuse id, add channel for uniqueness
                    LastModified = medium.timestamp,
                    Title = medium.title,
                    Topic = medium.topic,
                    Url = url,
                    Duration = medium.duration,
                    Size = medium.size,
                    MediaType = Mediathekar.Models.MediaType.Movie   // standard
                };
                uint? season = null;
                uint? episode = null;
                
                if (Mediathekar.Models.Utilities.MediaElementUtilities.ContainsSeasonEpisode(medium.title, ref season, ref episode))
                {
                    element.Season = season;
                    element.Episode = episode;
                    element.MediaType = Mediathekar.Models.MediaType.Series;
                }
                if(medium.duration < 60 * 60)
                {
                    // less than 60min -> not a movie
                    element.MediaType = Mediathekar.Models.MediaType.Series;
                }
                mediaElements.Add(element);
            }
            return mediaElements;
        }
    }
}