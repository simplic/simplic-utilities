using Simplic.Collections.Generic;
using System;
using System.Linq;

namespace Simplic.Lexer
{
    /// <summary>
    /// Tokenizer, which split the code into single tokens
    /// </summary>
    public class Tokenizer
    {
        #region Private Member

        private Dequeue<string> tokens;
        private ILexerConstants lexerConstants;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create
        /// </summary>
        public Tokenizer(ILexerConstants lexerConstants)
        {
            tokens = new Dequeue<string>();
            this.lexerConstants = lexerConstants;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Parse script into tokens
        /// </summary>
        /// <param name="Code"></param>
        public void Parse(string Code)
        {
            string lastToken = "";
            bool lastTokenIsSeperator = false;

            // Add line-break at the beginning and end
            Code = Environment.NewLine + Code;

            for (int i = 0; i < Code.Length; i++)
            {
                char currentChar = Code[i];

                // Add Seperators directly as a token
                if (lexerConstants.TokenSeperator.Contains(currentChar))
                {
                    if (lastToken.Trim() != "")
                    {
                        tokens.PushBack(lastToken.Trim());
                        lastToken = "";
                    }

                    if (currentChar != ' ')
                    {
                        string toEnqueu = currentChar.ToString();

                        // Make from two seperate tokens (> =) one token >=
                        if (lastTokenIsSeperator == true)
                        {
                            string lastTokenChar = tokens.PeekLast();

                            foreach (string fol in lexerConstants.FollowingTokens)
                            {
                                if ((lastTokenChar + currentChar) == fol)
                                {
                                    tokens.PopLast();
                                    toEnqueu = fol;
                                    break;
                                }
                            }
                        }

                        tokens.PushBack(toEnqueu);
                        lastTokenIsSeperator = true;
                    }
                }
                else if (lexerConstants.ComplexToken.Contains(currentChar))
                {
                    if (lastToken.Trim() != "")
                    {
                        tokens.PushBack(lastToken.Trim());
                        lastToken = "";
                    }

                    // Get Brackets like quoated strings
                    QuotedParameterParserResult result = GetNextComplexString(Code.Substring(i, Code.Length - i), currentChar);
                    tokens.PushBack(result.Result.Trim());

                    i += (result.Result.Length - 1) + result.RemovedChars;

                    lastTokenIsSeperator = false;
                }
                else if (currentChar == '\t')
                {
                    // Do nothing then
                    if (lastToken.Trim() != "")
                    {
                        tokens.PushBack(lastToken.Trim());
                        lastToken = "";
                    }
                }
                else
                {
                    lastToken += currentChar;

                    lastTokenIsSeperator = false;
                }
            }

            if (lastToken.Trim().Length > 0)
            {
                tokens.PushBack(lastToken.Trim());
            }
        }

        #endregion Public Methods

        #region Private Methods

        private QuotedParameterParserResult GetNextComplexString(string Input, char StartEndChar)
        {
            // Define return-value / vars
            QuotedParameterParserResult returnValue = new QuotedParameterParserResult();
            returnValue.RemovedChars = 0;
            returnValue.Result = "";

            bool addNextDirect = false;
            int unescapedQuotes = 0;

            for (int i = 0; i < Input.Length; i++)
            {
                if (addNextDirect)
                {
                    returnValue.Result += Input[i];

                    addNextDirect = false;
                    continue;
                }

                if (Input[i] == StartEndChar)
                {
                    unescapedQuotes++;
                }

                if (Input[i] == '\\')
                {
                    // This char will not be added to the result
                    returnValue.RemovedChars++;

                    addNextDirect = true;
                    continue;
                }
                returnValue.Result += Input[i];

                // Leave if all unescaped quoates are closed
                if (unescapedQuotes == 2)
                {
                    returnValue.Result = returnValue.Result.Substring(0, (i - returnValue.RemovedChars) + 1);
                    break;
                }
            }

            if (unescapedQuotes % 2 != 0)
            {
                throw new Exception(message: "Syntax-Error: Not all " + StartEndChar + " are closed");
            }

            return returnValue;
        }

        #endregion Private Methods

        #region Public Member

        /// <summary>
        /// Tokens for the current tokenizer
        /// </summary>
        public Dequeue<string> Tokens
        {
            get { return tokens; }
            set { tokens = value; }
        }

        #endregion Public Member
    }
}