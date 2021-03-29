using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediathekar.Channels
{
    interface IQueryableChannelClient
    {
        public Task<List<Mediathekar.Models.MediaElement>> SearchForMediaElements(string searchterm);
    }
}
