using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using QueryData;
using ResultData;

namespace MediaLibrarian.Services
{

    public class MediathekViewWebService
    {
        public MediathekViewWebService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            _query = new Query();
        }

        public IWebHostEnvironment WebHostEnvironment { get; }
        private MediathekViewWebService() => _query = new Query();
        private async Task<MediathekResult> sendQuery()
        {
            try
            {
                return await ProcessQuery(_query);
            }
            catch (Exception e)
            {
                return new MediathekResult { err = e.Message };
            }
        }

        public async Task<List<MediaLibrarian.Models.MediaElement>> GetMediaElements()
        {
            _query.size = 10000;
            _query.sortBy = "timestamp";
            _query.sortOrder = "desc";
            var result = await sendQuery();
            return MediathekResult.ToMediaElements(result);
        }

        private Query _query;
        private static readonly HttpClient client = new HttpClient();



        private static async Task<MediathekResult> ProcessQuery(Query query)
        {
            JsonSerializerOptions querySerializerOptions = new JsonSerializerOptions { IgnoreNullValues = true };
            var text = JsonSerializer.Serialize(query, querySerializerOptions);
            var httpContent = new StringContent(text, Encoding.UTF8, "text/plain");
            var streamTask = await client.PostAsync("https://mediathekviewweb.de/api/query", httpContent);
            if (streamTask.IsSuccessStatusCode)
            {
                var stream = await streamTask.Content.ReadAsStreamAsync();
                JsonSerializerOptions resultSerializerOptions = new JsonSerializerOptions();
                resultSerializerOptions.Converters.Add(new Models.Utilities.DateTimeConverter());
                resultSerializerOptions.Converters.Add(new Models.Utilities.NullableUInt32Converter());
                MediathekResult res = null;
                try
                {
                    res = await JsonSerializer.DeserializeAsync<MediathekResult>(stream, resultSerializerOptions);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return res;
            }
            return new MediathekResult { err = streamTask.ReasonPhrase };
        }
    }
}
