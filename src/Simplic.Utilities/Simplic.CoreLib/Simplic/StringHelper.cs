using System;
using System.Text;

namespace Simplic
{
    /// <summary>
    /// Provide functions for string operations
    /// </summary>
    public class StringHelper
    {
        #region Public Methods

        /// <summary>
        /// Get Null for empty/whitespace string
        /// </summary>
        /// <param name="input">Text</param>
        /// <returns>Null or input string</returns>
        public static string GetNullForWhiteSpace(string input)
        {
            return String.IsNullOrWhiteSpace(input) ? null : input;
        }

        /// <summary>
        /// Break string into lines
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="MaxCharsInLine"></param>
        /// <returns></returns>
        public static string BreakLine(string Text, int MaxCharsInLine)
        {
            int charsInLine = 0;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                if (char.IsWhiteSpace(c) || charsInLine >= MaxCharsInLine)
                {
                    builder.AppendLine();
                    charsInLine = 0;
                }
                else
                {
                    builder.Append(c);
                    charsInLine++;
                }
            }
            return builder.ToString();
        }

        #endregion Public Methods
    }
}