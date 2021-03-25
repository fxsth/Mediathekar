using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mediathekar.Channels;
using Mediathekar.Models;

namespace Mediathekar.Services
{
    public class ChannelClientService
    {
        public List<MediaElement> GetLatestMediaElements()
        {
            List<MediaElement> channelClientResults = new List<MediaElement>();
            List<Task<List<MediaElement>>> tasks = new List<Task<List<MediaElement>>>();
            foreach(var client in ChannelClients)
            {
                tasks.Add(client.GetLatestMediaElements());
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks)
            {
                channelClientResults.AddRange(task.Result);
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
