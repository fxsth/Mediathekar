using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MediaLibrarian.Models;
using Microsoft.AspNetCore.Hosting;

namespace MediaLibrarian.Services
{
    public class PokemonTVDataService
    {
        public PokemonTVDataService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        public IEnumerable<PokemonTVResult> GetResultData()
        {
            return RetrieveAvailableData().Result;
        }
        public async Task<IList<MediaLibrarian.Models.Medium>?> GetAllMedia()
        {
            var PokemonResultData = await RetrieveAvailableData();
            IList<MediaLibrarian.Models.Medium> allMedia = PokemonResultData[0].media;
            for (int i = 0; i < PokemonResultData.Count; i++)
            {
                if (i > 0)
                {
                    foreach (var episode in PokemonResultData[i].media)
                    {
                        allMedia.Add(episode);
                    }
                }
            }
            return allMedia;
        }
        public async Task<IList<PokemonTVResult>?> RetrieveAvailableData()
        {
            try
            {
                var streamTask = await client.GetAsync("https://www.pokemon.com/api/pokemontv/v2/channels/de/");
                if (streamTask.IsSuccessStatusCode)
                {
                    string stringResult = await streamTask.Content.ReadAsStringAsync();
                    JsonSerializerOptions resultSerializerOptions = new JsonSerializerOptions();
                    resultSerializerOptions.Converters.Add(new Models.Utilities.DateTimeConverter());
                    var res = JsonSerializer.Deserialize<IList<PokemonTVResult>>(stringResult, resultSerializerOptions);
                    //res.Sort(CompareResultByUpdateDate);
                    return res;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private static int CompareResultByUpdateDate(PokemonTVResult X, PokemonTVResult Y)
        {
            var x = X.channel_update_date;
            var y = Y.channel_update_date;
            return y.CompareTo(x);
        }

        private static readonly HttpClient client = new HttpClient();
    }
}