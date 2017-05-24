using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.CommandShell
{
    /// <summary>
    /// The shell context is the core of the command execution. Here are all commands registred. A ShellContext can be created of the ShellManager
    /// </summary>
    public class CommandShellContext
    {
        #region Private member
        // Unique name of the context
        private string contextName;

        // List with all shell commands
        private IDictionary<string, CommandShellCommand> shellCommands;
        #endregion

        #region Constructor
        /// <summary>
        /// Create new shell context
        /// </summary>
        /// <param name="contextName"></param>
        public CommandShellContext(string contextName)
        {
            this.contextName = contextName;
            shellCommands = new Dictionary<string, CommandShellCommand>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Register a method to the shell context
        /// </summary>
        /// <param name="cmdName">name of the cmd, must be unique in one shell context</param>
        /// <param name="methodDelegate">Delegate to the .Net method</param>
        /// <param name="requiredParameter">name of the required parameter, so that the system will automatically return an error if not all parameter were passed</param>
        public void RegisterMethod(string cmdName, CommandShellCommandDelegate methodDelegate, params string[] requiredParameter)
        {
            CommandShellCommand command = new CommandShellCommand(cmdName, methodDelegate, requiredParameter);

            if (shellCommands.ContainsKey(cmdName.ToLower()))
            {
                throw new ArgumentException(String.Format("The command {0} in the shell context {1} already exists", cmdName, contextName));
            }

            // add the command to the list of all commands
            shellCommands.Add(cmdName.ToLower(), command);
        }

        /// <summary>
        /// Register a static method to the shell context
        /// </summary>
        /// <param name="cmdName">name of the cmd, must be unique in one shell context</param>
        /// <param name="classType">Type of the static class or the class instance</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="requiredParameter">All parameter</param>
        public void RegisterStaticMethod(string cmdName, Type classType, string methodName, string[] requiredParameter)
        {
            CommandShellCommand command = new CommandShellCommand(cmdName, classType, methodName, requiredParameter);

            if (shellCommands.ContainsKey(cmdName.ToLower()))
            {
                throw new ArgumentException(String.Format("The command {0} in the shell context {1} already exists", cmdName, contextName));
            }

            // add the command to the list of all commands
            shellCommands.Add(cmdName.ToLower(), command);
        }

        /// <summary>
        /// Register a method to the shell context
        /// </summary>
        /// <param name="cmdName">name of the cmd, must be unique in one shell context</param>
        /// <param name="classInstance">Instance of a class</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="requiredParameter">All parameter</param>
        public void RegisterMethod(string cmdName, object classInstance, string methodName, string[] requiredParameter)
        {
            CommandShellCommand command = new CommandShellCommand(cmdName, classInstance, methodName, requiredParameter);

            if (shellCommands.ContainsKey(cmdName.ToLower()))
            {
                throw new ArgumentException(String.Format("The command {0} in the shell context {1} already exists", cmdName, contextName));
            }

            // add the command to the list of all commands
            shellCommands.Add(cmdName.ToLower(), command);
        }

        /// <summary>
        /// Execute a shell command
        /// </summary>
        /// <param name="command">Command line content</param>
        /// <param name="errorOccured">Out parameter which defines wether an error occured</param>
        /// <returns>Return-Message of the execute method</returns>
        public string Execute(string command, out bool errorOccured)
        {
            errorOccured = false;

            if (command.Trim() == "?")
            {
                StringBuilder cmdList = new StringBuilder();

                foreach (var lstCommand in shellCommands.OrderBy(Item => Item.Key).ToList())
                {
                    cmdList.AppendLine(lstCommand.Value.GetHelpText());
                }

                return cmdList.ToString();
            }
            if (command.StartsWith("?"))
            {
                command = command.Replace("?", "");

                if (shellCommands.ContainsKey(command.ToLower()))
                {
                    return shellCommands[command.ToLower()].GetMethodHelpText();
                }

                return "Command '" + command + "' not found";
            }
            else if (command.StartsWith("!"))
            {
                command = command.Replace("!", "").Trim();

                CommandShellManager.Singleton.ActivateShellContext(command);
                return "Switch context";
            }

            try
            {
                var result = Parser.ShellCommandParser.ParseCommand(command);

                string cmdName = result.Item1;
                CommandShellParameter[] parameter = result.Item2.ToArray();

                if (!shellCommands.ContainsKey(cmdName.ToLower()))
                {
                    errorOccured = true;
                    return String.Format("Command '{0}' not found.", cmdName);
                }

                CommandShellCommand cmd = shellCommands[cmdName.ToLower()];

                // Proof wether all required parameter exists
                if (cmd.RequiredParameter != null)
                {
                    foreach (string param in cmd.RequiredParameter)
                    {
                        bool paramFound = false;

                        foreach (CommandShellParameter shellParam in parameter)
                        {
                            if (shellParam.Name.ToLower() == param.ToLower())
                            {
                                paramFound = true;
                            }
                        }

                        if (!paramFound)
                        {
                            errorOccured = true;
                            return "Missing required parameter '" + param + "'";
                        }
                    }
                }

                return cmd.Execute(parameter);
            }
            catch (Exception ex)
            {
                errorOccured = false;
                return ex.Message;
            }
        }

        /// <summary>
        /// Execute multiple commands
        /// </summary>
        /// <param name="commands">String with commands</param>
        /// <param name="continueOnError">Continue if one of the commands failed</param>
        /// <param name="errorOccured">Continues if an error occurse</param>
        /// <returns>Result-String</returns>
        public string ExecuteMultiline(string commands, bool continueOnError, out bool errorOccured)
        {
            errorOccured = false;
            StringBuilder returnValue = new StringBuilder();

            string[] lines = commands.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (line.Trim().StartsWith("#") || String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                bool cmdErrorOccured = false;
                returnValue.AppendLine(Execute(line.Trim(), out errorOccured));

                if (cmdErrorOccured == true && continueOnError == false)
                {
                    errorOccured = true;
                    break;
                }
            }

            return returnValue.ToString();
        }
        #endregion

        #region Private Methods

        #endregion

        #region Public Member
        /// <summary>
        /// Unique name of the context
        /// </summary>
        public string ContextName
        {
            get { return contextName; }
        }
        #endregion
    }
}
