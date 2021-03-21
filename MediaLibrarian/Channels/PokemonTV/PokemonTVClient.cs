using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaLibrarian.Channels.PokemonTV
{
    public class PokemonTVClient : IChannelClient
    {
        public async Task<List<MediaLibrarian.Models.MediaElement>> GetLatestMediaElements()
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
                    resultSerializerOptions.Converters.Add(new Models.Utilities.NullableDateTimeConverter());
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