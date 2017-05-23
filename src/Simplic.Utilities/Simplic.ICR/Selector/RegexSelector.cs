using System.Text.RegularExpressions;

namespace Simplic.ICR.Selector
{
    /// <summary>
    /// Create regular expression (regex) selector
    /// </summary>
    public class RegexSelector : ISelector
    {
        private Regex regex;

        /// <summary>
        /// Regex as string
        /// </summary>
        /// <param name="configuration">Regular expression string</param>
        public void Initialize(string configuration)
        {
            regex = new Regex(configuration, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Execute the regular expression and return result of the match (true/false)
        /// </summary>
        /// <param name="input">String to proof</param>
        /// <returns>True if the string matchs</returns>
        public bool Compare(string input, out string _out)
        {
            var res = regex.IsMatch(input);
            _out = null;

            if (res)
            {
                _out = input;
            }

            return res;
        }
    }
}