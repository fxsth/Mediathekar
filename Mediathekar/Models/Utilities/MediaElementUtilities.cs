using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mediathekar.Models.Utilities
{
    public class MediaElementUtilities
    {
        public static bool ContainsSeasonEpisode(string title, ref uint? season, ref uint? episode)
        {
            season = null; episode = null;
            if (string.IsNullOrWhiteSpace(title))
                return false;
            string pattern = @"([S|s]\d\d.?[E|e]\d\d)";
            var match = Regex.Match(title, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            string[] numbers = Regex.Split(match.Value, @"\D+");
            if (numbers.Length == 2 && !string.IsNullOrWhiteSpace(numbers[0]) && !string.IsNullOrWhiteSpace(numbers[1]))
            {
                season = uint.Parse(numbers[0]);
                episode = uint.Parse(numbers[1]);
                return true;
            }
            pattern = @"[(]\d+[)]";
            match = Regex.Match(title, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            if (match.Success)
            {
                uint n = uint.Parse(match.Value.Substring(1, match.Value.Length - 2));
                if (n < 1800)
                {
                    episode = n;
                    return true;
                }
            }
            return false;

        }
    }
}
