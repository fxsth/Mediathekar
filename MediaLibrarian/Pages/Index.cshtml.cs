using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MediaLibrarian.Data;
using MediaLibrarian.Models;
using MediaLibrarian.Services;
using System.Diagnostics;

namespace MediaLibrarian.Pages
{
    public class MediaElementsModel : PageModel
    {
        private readonly MediaLibrarian.Data.MediaElementContext _context;
        public PokemonTVDataService PokemonTVDataService;
        public MediathekViewWebService MediathekViewWebService;

        public MediaElementsModel
            (MediaLibrarian.Data.MediaElementContext context,
            PokemonTVDataService pokemonTVDataService,
            MediathekViewWebService mediathekViewWebService
            )
        {
            _context = context;
            PokemonTVDataService = pokemonTVDataService;
            MediathekViewWebService = mediathekViewWebService;
        }
        public string TitleSort { get; set; }
        public string DateSort { get; set; }
        public string SeasonSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<MediaElement> MediaElements { get; set; }

        public async Task OnGetAsync(string sortOrder,
        string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            // changing from asc to desc
            TitleSort = sortOrder == "Title" ? "title_desc" : "Title";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            SeasonSort = sortOrder == "Season" ? "season_desc" : "Season";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<MediaElement> mediaElements = from s in _context.MediaElements
                                                     select s;
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                if (searchString.StartsWith("!"))
                {
                    searchString.Trim();
                    var index = searchString.IndexOf(" ");
                    string channel = index == -1 ? searchString.Substring(1) : searchString.Substring(1, index - 1);
                    mediaElements = mediaElements.Where(s => s.Channel == channel && s.Topic.Contains(searchString.Substring(index + 1)));
                }
                else
                {
                    mediaElements = mediaElements.Where(s => s.Title.Contains(searchString)
                                           || s.Topic.Contains(searchString) || s.Channel.Contains(searchString));
                }
            }
            switch (sortOrder)
            {
                case "Title":
                    mediaElements = mediaElements.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    mediaElements = mediaElements.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    mediaElements = mediaElements.OrderBy(s => s.LastModified);
                    break;
                case "Season":
                    mediaElements = mediaElements.OrderBy(s => s.Season).ThenBy(s => s.Episode);
                    break;
                case "season_desc":
                    mediaElements = mediaElements.OrderByDescending(s => s.Season).ThenByDescending(s => s.Episode);
                    break;
                default:
                    mediaElements = mediaElements.OrderByDescending(s => s.LastModified);
                    break;

            }

            int pageSize = 100;
            MediaElements = await PaginatedList<MediaElement>.CreateAsync(
                mediaElements.AsNoTracking().AsNoTracking(), pageIndex ?? 1, pageSize);
        }
        public async Task OnPostAsync(string sortOrder,
        string currentFilter, string searchString, int? pageIndex)
        {
            var pokedex = await PokemonTVDataService.GetAllMedia();
            var mediathekList = await MediathekViewWebService.GetMediaElements();
            try
            {
                _context.AddRangeIfNotExists(pokedex);
                _context.AddRangeIfNotExists(mediathekList);
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                _context.Dispose();
            }
            _context.SaveChanges();

            int pageSize = 100;
            MediaElements = await PaginatedList<MediaElement>.CreateAsync(
                _context.MediaElements.AsNoTracking().AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
