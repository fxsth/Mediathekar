using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediathekar.Channels
{
    interface IChannelClient
    {
        public Task<List<Mediathekar.Models.MediaElement>> GetLatestMediaElements();
    }
}
