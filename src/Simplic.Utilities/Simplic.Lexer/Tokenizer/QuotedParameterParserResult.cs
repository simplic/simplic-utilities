namespace Simplic.Lexer
{
    /// <summary>
    /// Result of a parsed-parameter
    /// </summary>
    internal class QuotedParameterParserResult
    {
        public string Result
        {
            get;
            set;
        }

        public int RemovedChars
        {
            get;
            set;
        }
    }
}