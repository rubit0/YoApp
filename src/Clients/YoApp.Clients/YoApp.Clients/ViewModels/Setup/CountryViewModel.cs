using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace YoApp.Clients.ViewModels.Setup
{
    public class CountryViewModel
    {
        public string Name { get; set; }
        public string CallingCode { get; set; }
        public string CountryCode { get; set; }
        public string EmojiUnicode { get; set; }
        public string Language { get; set; }
        public Dictionary<string, string> Translations { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public string ToStringWithEmojiFlag()
        {
            return $"{GetEmojiFlag()}\t{Name}";
        }

        public string GetEmojiFlag()
        {
            var matches = Regex.Matches(EmojiUnicode, @"U\+\w{5}");
            var segments = new List<string>(matches.Count);

            foreach (Match match in matches)
                segments.Add(match.Value);

            var converted = ToUtf16FromUnicode(segments);

            return converted;
        }

        private static string ToUtf16FromUnicode(IEnumerable<string> unicodeCodePoints)
        {
            var result = new StringBuilder();

            foreach (var input in unicodeCodePoints)
            {
                //Beginn at position 2 to skip the "U+" prefix
                var codePoint = int.Parse(input.Substring(2), System.Globalization.NumberStyles.HexNumber);
                result.Append(char.ConvertFromUtf32(codePoint));
            }

            return result.ToString();
        }
    }
}
