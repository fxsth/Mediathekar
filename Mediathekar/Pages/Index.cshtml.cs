using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mediathekar.Data;
using Mediathekar.Models;
using Mediathekar.Services;
using System.Diagnostics;

namespace Mediathekar.Pages
{
    public class MediaElementsModel : PageModel
    {
        private readonly Mediathekar.Data.MediaElementContext _context;
        public ChannelClientService _channelClientService;

        public MediaElementsModel
            (Mediathekar.Data.MediaElementContext context,
            ChannelClientService ChannelClientService
            )
        {
            _context = context;
            _channelClientService = ChannelClientService;
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

            if (!String.IsNullOrEmpty(searchString))
            {
                pageIndex = 1;
                var searchElements = _channelClientService.SearchForMediaElements(searchString);
                try
                {
                    _context.AddRangeIfNotExists(searchElements);
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
                _context.SaveChanges();
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<MediaElement> mediaElements = from s in _context.MediaElements
                                                     select s;
            // Filter Keywords/chars and searchterm
            mediaElements = DataUtilities.FilterSearch(mediaElements, searchString);
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
                mediaElements.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
        public async Task OnPostAsync(string sortOrder,
        string currentFilter, string searchString, int? pageIndex)
        {
            var mediaElements = _channelClientService.GetLatestMediaElements();
            try
            {
                _context.AddRangeIfNotExists(mediaElements);
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
            _context.SaveChanges();

            int pageSize = 100;
            MediaElements = await PaginatedList<MediaElement>.CreateAsync(
                _context.MediaElements.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
