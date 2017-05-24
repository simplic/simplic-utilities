using Simplic.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.CommandShell.Parser
{
    /// <summary>
    /// Parse the commands
    /// </summary>
    public class ShellCommandParser
    {
        /// <summary>
        /// Parse single command
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Tuple with the parameter name and a list of parameter-values</returns>
        public static Tuple<string, List<CommandShellParameter>> ParseCommand(string input)
        {
            string cmdName = "";
            List<CommandShellParameter> parameter = new List<CommandShellParameter>();

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception(message: "Empty command is not valid");
            }

            // Begin parameter parsing
            Tokenizer tokenizer = new Tokenizer(new ParserConstants());

            tokenizer.Parse(input);

            // Get command
            cmdName = tokenizer.Tokens.PopFirst();

            string lastParamName = null;
            List<string> parameterValue = null;

            foreach (string token in tokenizer.Tokens)
            {
                if (token.StartsWith("--"))
                {
                    if (lastParamName != null)
                    {
                        parameter.Add(new CommandShellParameter(lastParamName, parameterValue.ToArray()));
                    }

                    lastParamName = token.Substring(2, token.Length - 2);
                    parameterValue = new List<string>();
                }
                else
                {
                    if (lastParamName == null)
                    {
                        throw new Exception("No parameter-name for value '" + token + "'");
                    }

                    if (token.Trim().StartsWith("\"") && token.Trim().EndsWith("\"") && token.Trim().Length >= 2)
                    {
                        parameterValue.Add(token.Substring(1, token.Length - 2));
                    }
                    else if (token.Trim().StartsWith("'") && token.Trim().EndsWith("'") && token.Trim().Length >= 2)
                    {
                        parameterValue.Add(token.Substring(1, token.Length - 2));
                    }
                    else
                    {
                        parameterValue.Add(token);
                    }

                }
            }

            // Add last parameter
            if (lastParamName != null)
            {
                parameter.Add(new CommandShellParameter(lastParamName, parameterValue.ToArray()));
            }

            return new Tuple<string, List<CommandShellParameter>>(cmdName, parameter);
        }
    }
}
