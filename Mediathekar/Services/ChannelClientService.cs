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
            List<MediaElement> results = new List<MediaElement>();
            List<Task<List<MediaElement>>> tasks = new List<Task<List<MediaElement>>>();
            foreach(var client in ChannelClients)
            {
                tasks.Add(client.GetLatestMediaElements());
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks)
            {
                results.AddRange(task.Result);
            }
            
            return results;
        }
        public List<MediaElement> SearchForMediaElements(string searchterm)
        {
            List<MediaElement> results = new List<MediaElement>();
            List<Task<List<MediaElement>>> tasks = new List<Task<List<MediaElement>>>();
            foreach (var client in QueryableChannelClients)
            {
                tasks.Add(client.SearchForMediaElements(searchterm));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks)
            {
                results.AddRange(task.Result);
            }

            return results;
        }

        public ChannelClientService()
        {
            ChannelClients = GetInstancesOfInterface<IChannelClient>();
            QueryableChannelClients = GetInstancesOfInterface<IQueryableChannelClient>();
        }

        // Gets all Classes that implement Interface T and create Instances
        private List<T> GetInstancesOfInterface<T>()
        {
            var InstanceList = new List<T>();
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
            foreach (var clientType in types)
            {
                T obj = (T)Activator.CreateInstance(clientType);
                InstanceList.Add(obj);
            }
            return InstanceList;
        }
        private List<IChannelClient> ChannelClients { get; set; }
        private List<IQueryableChannelClient> QueryableChannelClients { get; set; }
    }
}
