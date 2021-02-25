using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediaLibrarian.Models;
using System.Diagnostics.CodeAnalysis;

namespace MediaLibrarian.Data
{
    public class MediaElementContext : DbContext
    {
        public MediaElementContext(DbContextOptions<MediaElementContext> options)
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
        public void AddRangeIfNotExists(List<MediaElement> elements)
        {
            HashSet<MediaElement> existingElements = MediaElements.AsNoTracking().ToHashSet(new MediaElementComparer());
            HashSet<MediaElement> newElements = elements.ToHashSet(new MediaElementComparer());
            newElements.ExceptWith(existingElements);
            MediaElements.AddRange(newElements);
        }

        public DbSet<MediaLibrarian.Models.MediaElement> MediaElements { get; set; }
    }

    public class MediaElementComparer : IEqualityComparer<MediaElement>
    {
        public bool Equals([AllowNull] MediaElement x, [AllowNull] MediaElement y)
        {
            return x.IdInChannel == y.IdInChannel;
        }

        public int GetHashCode([DisallowNull] MediaElement obj)
        {
            return obj.IdInChannel.GetHashCode();
        }
    }
}
