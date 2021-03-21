using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaLibrarian.Channels.MediathekViewWeb
{

    public class MediathekViewWebClient : IChannelClient
    {
        public MediathekViewWebClient() => _query = new Query();
        private async Task<MediathekViewWebResult> sendQuery()
        {
            try
            {
                return await ProcessQuery(_query);
            }
            catch (Exception e)
            {
                return new MediathekViewWebResult { err = e.Message };
            }
        }

        public async Task<List<Models.MediaElement>> GetLatestMediaElements()
        {
            _query.size = 1000;
            _query.sortBy = "timestamp";
            _query.sortOrder = "desc";
            _query.future = false;
            var result = await sendQuery();
            return MediathekViewWebResult.ToMediaElements(result);
        }

        private Query _query;
        private static readonly HttpClient client = new HttpClient();

        private static async Task<MediathekViewWebResult> ProcessQuery(Query query)
        {
            JsonSerializerOptions querySerializerOptions = new JsonSerializerOptions { IgnoreNullValues = true };
            var text = JsonSerializer.Serialize(query, querySerializerOptions);
            var httpContent = new StringContent(text, Encoding.UTF8, "text/plain");
            var streamTask = await client.PostAsync("https://mediathekviewweb.de/api/query", httpContent);
            if (streamTask.IsSuccessStatusCode)
            {
                var stream = await streamTask.Content.ReadAsStreamAsync();
                JsonSerializerOptions resultSerializerOptions = new JsonSerializerOptions();
                resultSerializerOptions.Converters.Add(new Models.Utilities.NullableDateTimeConverter());
                resultSerializerOptions.Converters.Add(new Models.Utilities.NullableUInt32Converter());
                MediathekViewWebResult res = null;
                try
                {
                    res = await JsonSerializer.DeserializeAsync<MediathekViewWebResult>(stream, resultSerializerOptions);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return res;
            }
            return new MediathekViewWebResult { err = streamTask.ReasonPhrase };
        }
    }
}
