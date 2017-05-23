using System.Text.RegularExpressions;

namespace Simplic.Text.RegularExpressions
{
    /// <summary>
    /// Contains a list of default regular expressions, which can be used in the simplic framework. The getter of an
    /// RegexFilter will always return a new instance of a System.Text.RegularExpressions.Regex
    /// </summary>
    public class RegexFilter
    {
        /// <summary>
        /// Regex filter whole integer, no negative values
        /// </summary>
        public static Regex FilterUnsignedInteger
        {
            get
            {
                return new Regex("[1-9][0-9]*");
            }
        }

        /// <summary>
        /// Regex filter whole integer, containing negative values
        /// </summary>
        public static Regex FilterSignedInteger
        {
            get
            {
                return new Regex(@"-?[1-9][\d]*");
            }
        }

        /// <summary>
        /// Regex filter for float values, no negative values yet
        /// </summary>
        public static Regex FilterUnsignedFloat
        {
            get
            {
                return new Regex(@"^(?=.+)(?:[1-9]\d*|0)?(?:\.\d+)?$");
            }
        }
    }
}