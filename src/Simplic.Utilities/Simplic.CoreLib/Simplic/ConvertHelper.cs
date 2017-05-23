using System.Text;

namespace Simplic
{
    /// <summary>
    /// Convert Helpers
    /// </summary>
    public class ConvertHelper
    {
        /// <summary>
        /// Get Base64-String from String-Input (Use UTF-8 Encoding)
        /// </summary>
        /// <param name="input">Plain text</param>
        /// <returns>Base64 String</returns>
        public static string StringToBase64String(string input)
        {
            return StringToBase64String(input, Encoding.UTF8);
        }

        /// <summary>
        /// Get plaintext from Base64-string-input (Use UTF-8 Encoding)
        /// </summary>
        /// <param name="input">Plain text</param>
        /// <returns>Base64 String</returns>
        public static string StringFromBase64String(string input)
        {
            return StringFromBase64String(input, Encoding.UTF8);
        }

        /// <summary>
        /// Get Base64-String from String-Input
        /// </summary>
        /// <param name="input">Plain text</param>
        /// <param name="encoding">String encoding</param>
        /// <returns>Base64 String</returns>
        public static string StringToBase64String(string input, Encoding encoding)
        {
            return System.Convert.ToBase64String(encoding.GetBytes(input));
        }

        /// <summary>
        /// Get plaintext from Base64-string-input (Use UTF-8 Encoding)
        /// </summary>
        /// <param name="input">Plain text</param>
        /// <param name="encoding">String encoding</param>
        /// <returns>Base64 String</returns>
        public static string StringFromBase64String(string input, Encoding encoding)
        {
            return encoding.GetString(System.Convert.FromBase64String(input));
        }
    }
}