using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaLibrarian.Channels;
using MediaLibrarian.Models;

namespace MediaLibrarian.Services
{
    public class ChannelClientService
    {

        public List<MediaElement> GetLatestMediaElements()
        {
            List<MediaElement> channelClientResults = new List<MediaElement>();
            foreach(var client in ChannelClients)
            {
                channelClientResults.AddRange(client.GetLatestMediaElements().Result);
            }
            return channelClientResults;
        }

        public ChannelClientService()
        {
            ChannelClients = new List<IChannelClient>();
            // Get all Classes that implements IChannelClient
            var type = typeof(IChannelClient);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
            foreach(var clientType in types)
            {
                IChannelClient obj = (IChannelClient) Activator.CreateInstance(clientType);
                ChannelClients.Add(obj);
            }
        }
        private List<IChannelClient> ChannelClients { get; set; }
    }
}
