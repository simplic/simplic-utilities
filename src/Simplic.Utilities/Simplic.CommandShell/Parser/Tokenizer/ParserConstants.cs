using Simplic.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.CommandShell.Parser
{
    internal class ParserConstants : ILexerConstants
    {
        char[] ILexerConstants.TokenSeperator
        {
            get { return new char[] { ' ' }; }
        }

        string[] ILexerConstants.FollowingTokens
        {
            get { return new string[] { }; }
        }

        char[] ILexerConstants.ComplexToken
        {
            get
            {
                return new char[] { '"', '\'' };
            }
        }
    }
}
