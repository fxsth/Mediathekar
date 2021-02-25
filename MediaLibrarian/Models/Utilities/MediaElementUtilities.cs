using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MediaLibrarian.Models.Utilities
{
    public class MediaElementUtilities
    {
        public static bool ContainsSeasonEpisode(string title, ref int? season, ref int? episode)
        {
            season = null; episode = null;
            var trimmedTitle = title.Trim();
            if (string.IsNullOrWhiteSpace(trimmedTitle))
                return false;
            string pattern = @"([S|s]\d\d.?[E|e]\d\d)";
            var match = Regex.Match(trimmedTitle, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            var s = match.Value;
            string[] numbers = Regex.Split(s, @"\D+");
            if (numbers.Length == 2 && !string.IsNullOrWhiteSpace(numbers[0]) && !string.IsNullOrWhiteSpace(numbers[1]))
            {
                season = int.Parse(numbers[0]);
                episode = int.Parse(numbers[1]);
                return true;
            }
            return false;

        }
    }
}
