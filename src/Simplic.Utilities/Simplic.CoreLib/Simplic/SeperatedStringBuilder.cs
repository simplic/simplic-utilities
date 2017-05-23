using System.Text;

namespace Simplic
{
    /// <summary>
    /// Stringbuilder which automaticaly add a sparator
    /// </summary>
    public class SeperatedStringBuilder
    {
        private StringBuilder builder;
        private string separator;
        private string enclosingString;

        /// <summary>
        /// Initialize new seperated string builder
        /// </summary>
        /// <param name="separator">Seperator character which will be added before Append and AppendLine when
        /// text exists.</param>
        /// <param name="enclosingString">If an enclosing string is set, all added string will be wrapped around that string</param>
        public SeperatedStringBuilder(string separator, string enclosingString = "")
        {
            this.separator = separator;
            this.enclosingString = enclosingString;
            builder = new StringBuilder();
        }

        /// <summary>
        /// Append text
        /// </summary>
        /// <param name="text">Text to add</param>
        public void Append(string text)
        {
            if (builder.Length > 0)
            {
                builder.Append(separator);
            }

            builder.Append($"{enclosingString ?? ""}{text}{enclosingString ?? ""}");
        }

        /// <summary>
        /// Append text and add new line
        /// </summary>
        /// <param name="text">Text to add</param>
        public void AppendLine(string text)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine(separator);
            }

            builder.AppendLine($"{enclosingString ?? ""}{text}{enclosingString ?? ""}");
        }

        /// <summary>
        /// Get as string/text
        /// </summary>
        /// <returns>Content as string</returns>
        public override string ToString()
        {
            return builder.ToString();
        }
    }
}