using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mediathekar.Models
{
    public class MediaElement
    {
        [Key]
        public string IdInChannel { get; set; }
        public string Channel { get; set; }
        public string Topic { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public uint? Year { get; set; }
        public uint? Duration { get; set; }
        public uint? Size { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? OnlineFrom { get; set; }
        public DateTime? OnlineUntil { get; set; }
        public MediaType MediaType { get; set; }
        public uint? Season { get; set; }
        public uint? Episode { get; set; }

        public bool Equals(MediaElement x, MediaElement y)
        {
            return x.IdInChannel == y.IdInChannel;
        }
    }
    public enum MediaType
    {
        Movie,
        Series
    }
}
