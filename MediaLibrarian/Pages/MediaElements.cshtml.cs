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

        public MediaElementsModel
            (MediaLibrarian.Data.MediaElementContext context,
            PokemonTVDataService pokemonTVDataService
            )
        {
            _context = context;
            PokemonTVDataService = pokemonTVDataService;
        }

        public IList<MediaElement> MediaElements { get; set; }

        public async Task OnGetAsync()
        {
            MediaElements = await _context.MediaElements.ToListAsync();
        }
        public async Task OnPostAsync()
        {
            var list = await PokemonTVDataService.GetAllMedia();
            var mediaElements = new List<MediaElement>();
            foreach (var el in list)
            {
                try
                {
                    _context.AddIfNotExists(el.ToMediaElement());
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
            }
            _context.SaveChanges();
            MediaElements = await _context.MediaElements.ToListAsync();
        }
    }
}
