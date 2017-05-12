using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Utilities.TemplateParser
{
    /// <summary>
    /// Data template parser interface
    /// </summary>
    public interface ITemplateParser
    {
        /// <summary>
        /// Parses an input with given object values.
        /// </summary>
        /// <param name="input">Raw input string, i.e.: Hi {Name}</param>
        /// <param name="values">An array of objects to replace the variables in the input string</param>
        /// <returns>A parsed string with the values replaced.</returns>
        string ParseTemplate(string input, object[] values);

        /// <summary>
        /// Parses an input with given dictionary values.
        /// </summary>
        /// <param name="input">Raw input string, i.e.: Hi {Name}</param>
        /// <param name="values">A dictionary of values to replace the variables in the input string</param>
        /// <returns>A parsed string with the values replaced.</returns>
        string ParseTemplate(string input, IDictionary<string, string> values);

        /// <summary>
        /// Parses an input with given dictionary values.
        /// </summary>
        /// <param name="input">Raw input string, i.e.: Hi {Name}</param>
        /// <param name="values">A dictionary of values to replace the variables in the input string</param>
        /// <returns>A parsed string with the values replaced.</returns>
        string ParseTemplate(string input, IDictionary<string, object> values);
    }    
}
