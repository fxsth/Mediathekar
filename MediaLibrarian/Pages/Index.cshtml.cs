using System.Collections.Generic;
using MediaLibrarian.Models;
using MediaLibrarian.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MediaLibrarian.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public PokemonTVDataService PokemonTVDataService;
        public IEnumerable<PokemonTVResult> PokemonTVResults;

        public IndexModel(ILogger<IndexModel> logger,
            PokemonTVDataService pokemonTVDataService)
        {
            _logger = logger;
            PokemonTVDataService = pokemonTVDataService;
        }

        public void OnGet()
        {
            PokemonTVResults = PokemonTVDataService.GetResultData();
        }
    }
}
