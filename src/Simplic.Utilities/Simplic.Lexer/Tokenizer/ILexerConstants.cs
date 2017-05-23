namespace Simplic.Lexer
{
    /// <summary>
    /// Lexer constants, for plitting strings
    /// </summary>
    public interface ILexerConstants
    {
        /// <summary>
        /// Strings which seperates tokens
        /// </summary>
        char[] TokenSeperator
        {
            get;
        }

        /// <summary>
        /// Auto complete tokens, for example making >= out of > =
        /// </summary>
        string[] FollowingTokens
        {
            get;
        }

        /// <summary>
        /// Complex tokens, will keep tokens to gether, like "This is a complex token"
        /// </summary>
        char[] ComplexToken
        {
            get;
        }
    }
}