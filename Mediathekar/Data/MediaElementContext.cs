﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mediathekar.Models;
using System.Diagnostics.CodeAnalysis;

namespace Mediathekar.Data
{
    public class MediaElementContext : DbContext
    {
        public MediaElementContext(DbContextOptions<MediaElementContext> options)
            : base(options)
        {
        }
        public void AddRangeIfNotExists(List<MediaElement> elements)
        {
            if (elements != null)
            {
                HashSet<MediaElement> existingElements = MediaElements.AsNoTracking().ToHashSet(new MediaElementComparer());
                HashSet<MediaElement> newElements = elements.ToHashSet(new MediaElementComparer());
                newElements.ExceptWith(existingElements);
                MediaElements.AddRange(newElements);
            }
        }

        public DbSet<Mediathekar.Models.MediaElement> MediaElements { get; set; }
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
