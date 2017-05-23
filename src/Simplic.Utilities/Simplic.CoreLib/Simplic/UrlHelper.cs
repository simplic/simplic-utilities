using System;

namespace Simplic
{
    /// <summary>
    /// Provides functions to work with urls
    /// </summary>
    public class UrlHelper
    {
        /// <summary>
        /// Proof wether an url is in a correct http or https format
        /// </summary>
        /// <param name="url">Input url</param>
        /// <returns>True if the url is in a correct foramt, else false</returns>
        public static bool IsHttpHttpsUrl(string url)
        {
            Uri uriResult = null;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }
    }
}