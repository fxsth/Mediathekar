using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediaLibrarian.Models;

namespace MediaLibrarian.Data
{
    public class MediaElementContext : DbContext
    {
        public MediaElementContext (DbContextOptions<MediaElementContext> options)
            : base(options)
        {
        }
        public bool AddIfNotExists(MediaElement e)
        {
            var exists = MediaElements.Local.Any(localE => localE.IdInChannel == e.IdInChannel);
            var existsInDb = MediaElements.Any(dbE => dbE.IdInChannel == e.IdInChannel);
            if (!exists && !existsInDb)
                MediaElements.Add(e);
            return exists;
        }

        public DbSet<MediaLibrarian.Models.MediaElement> MediaElements { get; set; }
    }
}
