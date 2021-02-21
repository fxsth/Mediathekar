using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLibrarian.Models;
using MediaLibrarian.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MediaLibrarian.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public PokemonTVDataService PokemonTVDataService;
        public IList<MediaLibrarian.Models.Medium> MediaElements;
        public DownloadService DownloadService;

        public IndexModel(ILogger<IndexModel> logger,
            PokemonTVDataService pokemonTVDataService,
            DownloadService downloadService)
        {
            _logger = logger;
            PokemonTVDataService = pokemonTVDataService;
            DownloadService = downloadService;
        }

        public async Task OnGetAsync()
        {
            MediaElements = await PokemonTVDataService.GetAllMedia();
        }
        
    }
}
