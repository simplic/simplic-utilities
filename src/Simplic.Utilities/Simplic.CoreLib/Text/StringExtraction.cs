using System;
using System.Text.RegularExpressions;

namespace Simplic.Text
{
    /// <summary>
    /// Contains methods to extract data from strings
    /// </summary>
    public static class StringExtraction
    {
        /// <summary>
        /// Searches from date values in a string
        /// </summary>
        /// <param name="input">Input string that will be used for searching</param>
        /// <returns>Returns a <see cref="DateTime"/> instance of the text contains a date time in the format (dd.MM.yyyy HH:mm) HH:mm is optional</returns>
        public static DateTime? ExtractDateTime(string input)
        {
            var regex_date = new Regex(@"[0-9]{1,2}\.[0-9]{1,2}\.[0-9]{4}");
            var regex_dateTime = new Regex(@"[0-9]{2}\.[0-9]{2}\.[0-9]{4}\s[0-9]{2}\:[0-9]{2}");

            foreach (Match m in regex_dateTime.Matches(input))
            {
                DateTime dt;
                if (DateTime.TryParseExact(m.Value, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    return dt;
                }
            }

            foreach (Match m in regex_date.Matches(input))
            {
                DateTime dt;
                if (DateTime.TryParseExact(m.Value, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    return dt;
                }
            }

            return null;
        }
    }
}