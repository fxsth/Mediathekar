using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MediaLibrarian.Models;

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

    public class PokemonTVResultConverter
    {
        public static List<MediaElement> ToMediaElements(IEnumerable<PokemonTVResult> pokemonTVResults)
        {
            var elements = new List<MediaElement>();
            foreach (var res in pokemonTVResults)
            {
                if(res.media_type == "movie")
                    elements.AddRange(res.media.ConvertAll(new Converter<Medium, MediaElement>(PokemonMovieToMediaElement)));
                else
                    elements.AddRange(res.media.ConvertAll(new Converter<Medium, MediaElement>(PokemonEpisodeToMediaElement)));
            }
            return elements;
        }
        public static MediaElement PokemonEpisodeToMediaElement(Medium medium)
        {
            return new MediaElement
            {
                Channel = "PokemonTV",
                Episode = UInt32.TryParse(medium.episode, out var tempValE) ? tempValE : (uint?)null,
                Season = UInt32.TryParse(medium.season, out var tempValS) ? tempValS : (uint?)null,
                IdInChannel = "PokemonTV" + medium.id,
                LastModified = medium.last_modified,
                MediaType = MediaType.Series,
                Title = medium.title,
                Topic = "Pokemon",
                Url = medium.stream_url
            };
        }
        public static MediaElement PokemonMovieToMediaElement(Medium medium)
        {
            return new MediaElement
            {
                Channel = "PokemonTV",
                Episode = UInt32.TryParse(medium.episode, out var tempValE) ? tempValE : (uint?)null,
                Season = UInt32.TryParse(medium.season, out var tempValS) ? tempValS : (uint?)null,
                IdInChannel = "PokemonTV" + medium.id,
                LastModified = medium.last_modified,
                MediaType = MediaType.Movie,
                Title = medium.title,
                Topic = "Pokemon",
                Url = medium.stream_url
            };
        }
    }
}