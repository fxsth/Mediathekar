using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLibrarian.Models;
using MediaLibrarian.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MediaLibrarian.Pages
{
    public class DownloadModel : PageModel
    {
        private readonly DownloadService _downloadService;

        public List<DownloadFile> Downloads;

        public DownloadModel(DownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        //public void OnGet()
        //{
        //    Downloads = _downloadService.Downloads;
        //}
        public async Task<IActionResult> OnGetAsync(string season, string episode, string title, string url)
        {
            if (url != null && url.Length != 0)
            {

                _downloadService.Downloads.Add(new DownloadFile(url, title, episode == null && episode.Length == 0 ? MediaType.Movie :MediaType.Series));
            }
            Downloads = _downloadService.Downloads;
            return Page();
        }

        [BindProperty]
        public DownloadFile DownloadFile { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _downloadService.Downloads.Add(DownloadFile);
            Downloads = _downloadService.Downloads;

            return Page();
        }
    }
}
