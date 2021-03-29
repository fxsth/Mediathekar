using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediathekar.Models;
using Microsoft.EntityFrameworkCore;

namespace Mediathekar.Data
{
    public static class DataUtilities
    {
        // Filters IQueryable<MediaElements> by search arguments
        public static IQueryable<MediaElement> FilterSearch(IQueryable<MediaElement> mediaElements, string searchString)
        {
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                // Select Channel with !
                if (searchString.StartsWith("!"))
                {
                    searchString.Trim();
                    var index = searchString.IndexOf(" ");
                    string channel = index == -1 ? searchString.Substring(1) : searchString.Substring(1, index - 1);
                    mediaElements = mediaElements.Where(s => s.Channel.ToUpper().Equals(channel.ToUpper()));
                    searchString = index == -1 ? "" : searchString.Substring(index).Trim();
                    if (!string.IsNullOrWhiteSpace(searchString))
                    {
                        mediaElements = mediaElements.Where(s => EF.Functions.Like(s.Title, searchString)
                                               || EF.Functions.Like(s.Title, searchString));
                    }
                }
                // Select Topic with #
                else if (searchString.StartsWith("#"))
                {
                    searchString.Trim();
                    var index = searchString.IndexOf(" ");
                    string topic = index == -1 ? searchString.Substring(1) : searchString.Substring(1, index - 1);
                    mediaElements = mediaElements.Where(s => s.Topic.ToUpper().Equals(topic.ToUpper()));
                    searchString = index == -1 ? "" : searchString.Substring(index).Trim();
                    if (!string.IsNullOrWhiteSpace(searchString))
                    {
                        mediaElements = mediaElements.Where(s => EF.Functions.Like(s.Topic, searchString));
                    }
                }
                // Select Mediatype with movie/Movie
                else if (searchString.StartsWith("movie", true, null))
                {
                    searchString.Trim();
                    var index = searchString.IndexOf(" ");
                    string topic = index == -1 ? searchString.Substring(1) : searchString.Substring(1, index - 1);
                    mediaElements = mediaElements.Where(s => s.MediaType == MediaType.Movie);
                    searchString = index == -1 ? "" : searchString.Substring(index).Trim();
                    if (!string.IsNullOrWhiteSpace(searchString))
                    {
                        mediaElements = mediaElements.Where(s => EF.Functions.Like(s.Title, searchString)
                                               || EF.Functions.Like(s.Topic, searchString));
                    }
                }
                // Select Mediatype with series/Series
                else if (searchString.StartsWith("series", true, null))
                {
                    searchString.Trim();
                    var index = searchString.IndexOf(" ");
                    string topic = index == -1 ? searchString.Substring(1) : searchString.Substring(1, index - 1);
                    mediaElements = mediaElements.Where(s => s.MediaType == MediaType.Series);
                    searchString = index == -1 ? "" : searchString.Substring(index).Trim();
                    if (!string.IsNullOrWhiteSpace(searchString))
                    {
                        mediaElements = mediaElements.Where(s => EF.Functions.Like(s.Title, searchString)
                                               || EF.Functions.Like(s.Topic, searchString));
                    }
                }
                else
                {
                    mediaElements = mediaElements.Where(s => EF.Functions.Like(s.Title, searchString)
                                           || EF.Functions.Like(s.Topic, searchString)
                                           || EF.Functions.Like(s.Channel, searchString));
                }
            }
            return mediaElements;
        }
    }
}
