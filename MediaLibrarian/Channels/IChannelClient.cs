using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaLibrarian.Channels
{
    interface IChannelClient
    {
        public Task<List<MediaLibrarian.Models.MediaElement>> GetLatestMediaElements();
    }
}
