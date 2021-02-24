using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaLibrarian.Models
{
    public class MediaElement
    {
        [Key]
        public string IdInChannel { get; set; }
        public string Channel { get; set; }
        public string Topic { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int? Year { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime? OnlineFrom { get; set; }
        public DateTime? OnlineUntil { get; set; }
        public MediaType MediaType { get; set; }
        public int? Season { get; set; }
        public int? Episode { get; set; }
    }
}
