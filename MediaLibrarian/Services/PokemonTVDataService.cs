using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MediaLibrarian.Models;
using Microsoft.AspNetCore.Hosting;
using MediaLibrarian.Channel;

namespace MediaLibrarian.Services
{
    public class PokemonTVDataService
    {
        public PokemonTVDataService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }
        public async Task<List<MediaLibrarian.Models.MediaElement>> GetAllMedia()
        {
            var PokemonResultData = await RetrieveAvailableData();
            return PokemonTVResultConverter.ToMediaElements(PokemonResultData);
        }
        public async Task<IEnumerable<PokemonTVResult>> RetrieveAvailableData()
        {
            try
            {
                var streamTask = await client.GetAsync("https://www.pokemon.com/api/pokemontv/v2/channels/de/");
                if (streamTask.IsSuccessStatusCode)
                {
                    var stream = await streamTask.Content.ReadAsStreamAsync();
                    JsonSerializerOptions resultSerializerOptions = new JsonSerializerOptions();
                    resultSerializerOptions.Converters.Add(new Models.Utilities.DateTimeConverter());
                    return await JsonSerializer.DeserializeAsync<IEnumerable<PokemonTVResult>>(stream, resultSerializerOptions);
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