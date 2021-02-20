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

        public async Task<IEnumerable<PokemonTVResult>?> RetrieveAvailableData()
        {
            try
            {
                var streamTask = await client.GetAsync("https://www.pokemon.com/api/pokemontv/v2/channels/de/");
                if (streamTask.IsSuccessStatusCode)
                {
                    string stringResult = await streamTask.Content.ReadAsStringAsync();
                    JsonSerializerOptions resultSerializerOptions = new JsonSerializerOptions();
                    //resultSerializerOptions.IncludeFields = true;
                    var res = JsonSerializer.Deserialize<List<PokemonTVResult>>(stringResult, resultSerializerOptions);
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

        private static readonly HttpClient client = new HttpClient();
    }
}