﻿using System;
using System.Collections.Generic;
using MediaLibrarian.Models;

namespace MediaLibrarian.Channel
{
    public class Images
    {
        public string small { get; set; }
        public string large { get; set; }
        public string medium { get; set; }
    }

    public class Medium
    {
        public int count { get; set; }
        public int rating { get; set; }
        public string episode { get; set; }
        public string description { get; set; }
        public bool is_country_whitelist { get; set; }
        public string title { get; set; }
        public string season { get; set; }
        public Images images { get; set; }
        public string skimming_thumbnail_url_base { get; set; }
        public bool is_new { get; set; }
        public List<string> country_codes { get; set; }
        public string offline_url { get; set; }
        public DateTime last_modified { get; set; }
        public string stream_url { get; set; }
        public string captions { get; set; }
        public string id { get; set; }
        public object size { get; set; }
    }

    public class ChannelImages
    {
        public string dashboard_image_1125_1500 { get; set; }
        public string spotlight_image_960_1277 { get; set; }
        public string spotlight_image_2048_1152 { get; set; }
        public string spotlight_image_2732_940 { get; set; }
    }

    public class PokemonTVResult
    {
        public string channel_description { get; set; }
        public bool stunt_channel { get; set; }
        public string channel_name { get; set; }
        public List<Medium> media { get; set; }
        public DateTime channel_creation_date { get; set; }
        public int watch_now_order { get; set; }
        public string channel_id { get; set; }
        public ChannelImages channel_images { get; set; }
        public DateTime channel_update_date { get; set; }
        public string media_type { get; set; }
        public string channel_status { get; set; }
    }

    public class PokemonTVResultConverter
    {
        public static List<MediaElement> ToMediaElements(IEnumerable<PokemonTVResult> pokemonTVResults)
        {
            var elements = new List<MediaElement>();
            foreach (var res in pokemonTVResults)
            {
                elements.AddRange(res.media.ConvertAll(new Converter<Medium, MediaElement>(PokemonToMediaElement)));
            }
            return elements;
        }
        public static MediaElement PokemonToMediaElement(Medium medium)
        {
            return new MediaElement
            {
                Channel = "PokemonTV",
                Episode = Int32.TryParse(medium.episode, out var tempValE) ? tempValE : (int?)null,
                Season = Int32.TryParse(medium.season, out var tempValS) ? tempValS : (int?)null,
                IdInChannel = "PokemonTV" + medium.id,
                LastModified = medium.last_modified,
                MediaType = medium.episode.Length == 0 ? MediaType.Movie : MediaType.Series,
                Title = medium.title,
                Topic = "Pokemon",
                Url = medium.stream_url
            };
        }
    }
}