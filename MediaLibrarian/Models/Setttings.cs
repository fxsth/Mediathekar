using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaLibrarian.Models
{
    public class Settings
    {
        public string MoviesDir { get; set; }
        public string SeriesDir { get; set; }
        public bool RetrieveDataFrequently { get; set; }
        public bool RetrieveDataWithSearch { get; set; }
    }
}
