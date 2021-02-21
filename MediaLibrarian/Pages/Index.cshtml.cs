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
        public IEnumerable<PokemonTVResult> PokemonTVResults;
        public DownloadService DownloadService;

        public IndexModel(ILogger<IndexModel> logger,
            PokemonTVDataService pokemonTVDataService,
            DownloadService downloadService)
        {
            _logger = logger;
            PokemonTVDataService = pokemonTVDataService;
            DownloadService = downloadService;
        }

        public void OnGet()
        {
            PokemonTVResults = PokemonTVDataService.GetResultData();
        }
        
        public IActionResult StartDownload(string button)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            DownloadService.Downloads.Add(new DownloadFile(button, 0));
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string value)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //DownloadService.Downloads.Add(DownloadFile);

            return Page();
        }
    }
}
