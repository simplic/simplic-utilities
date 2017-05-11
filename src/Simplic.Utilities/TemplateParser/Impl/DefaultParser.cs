using SmartFormat;
using System.Collections.Generic;

namespace TemplateParser.Impl
{
    public class DefaultParser : ITemplateParser
    {
        /// <summary>
        /// This method removes old template syntax and returns the new one
        /// </summary>
        /// <param name="input">Template text to be stripped</param>        
        private string StripSyntax(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("${", "{");
        }


        /// <summary>
        /// Parses an input with given object values.
        /// </summary>
        /// <param name="input">Raw input string, i.e.: Hi {Name}</param>
        /// <param name="values">An array of objects to replace the variables in the input string</param>
        /// <returns>A parsed string with the values replaced.</returns>
        public string ParseTemplate(string input, object[] values)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            input = StripSyntax(input);
            return Smart.Format(input, values);
        }

        /// <summary>
        /// Parses an input with given dictionary values.
        /// </summary>
        /// <param name="input">Raw input string, i.e.: Hi {Name}</param>
        /// <param name="values">A dictionary of values to replace the variables in the input string</param>
        /// <returns>A parsed string with the values replaced.</returns>
        public string ParseTemplate(string input, IDictionary<string, string> values)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            input = StripSyntax(input);
            return Smart.Format(input, values);
        }
    }
}
