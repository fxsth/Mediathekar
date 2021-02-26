using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MediaLibrarian.Models;

namespace MediaLibrarian.Pages
{
    public class SettingsModel : PageModel
    {
        // requires using Microsoft.Extensions.Configuration;
        private readonly IConfiguration Configuration;

        public SettingsModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Settings settings { get; set; }

        public void OnGet()
        {
            Configuration.GetSection("Settings").Bind(settings);
        }
    }
}
