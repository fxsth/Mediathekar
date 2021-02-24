using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly MediaLibrarian.Data.MediaElementContext _context; 
        private readonly DownloadService _downloadService;

        public Queue<DownloadFile> Downloads;

        public DownloadModel(DownloadService downloadService,
            MediaLibrarian.Data.MediaElementContext context)
        {
            _context = context;
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
                if (!_downloadService.existsAlready(url))
                    _downloadService.addToDownloadQueue(new DownloadFile(url, title, episode == null || episode.Length == 0 ? MediaType.Movie : MediaType.Series));
            }
            Downloads = _downloadService.Downloads;
            return Page();
        }
        public async Task<IActionResult> OnGetMediaelementAsync(string id)
        {
            if (id == null || id.Length == 0)
            {
                return Page();
            }
            MediaElement toDownload;
            var bytes = Encoding.UTF8.GetBytes("Download failed - No Entity found");
            try
            {
                toDownload =  _context.MediaElements.Single(e => e.IdInChannel == id);
            }
            catch(Exception e)
            {
                return Page();
            }
            if (!_downloadService.existsAlready(toDownload.Url))
                _downloadService.addToDownloadQueue(new DownloadFile(toDownload));
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

            _downloadService.addToDownloadQueue(DownloadFile);
            Downloads = _downloadService.Downloads;

            return Page();
        }
    }
}
