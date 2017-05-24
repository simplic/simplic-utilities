using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.CommandShell
{
    /// <summary>
    /// The shell manager is the root of the shell lib framework.
    /// </summary>
    public class CommandShellManager
    {
        #region Singleton
        private static readonly CommandShellManager singleton = new CommandShellManager();

        /// <summary>
        /// Singleton access to the shell manager instance
        /// </summary>
        public static CommandShellManager Singleton
        {
            get { return CommandShellManager.singleton; }
        }
        #endregion

        #region Private Member
        // list of all available shell contexts
        IDictionary<string, CommandShellContext> shellContext;

        // Queue with active shell contexts
        private Stack<CommandShellContext> activatedShellContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Create the shell manager
        /// </summary>
        private CommandShellManager()
        {
            shellContext = new Dictionary<string, CommandShellContext>();
            activatedShellContext = new Stack<CommandShellContext>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Execute program arguments as a shell command
        /// </summary>
        /// <param name="cmdName">Command name</param>
        /// <param name="args">Arguments</param>
        /// <param name="errorOccured">Out parameter which defines wether an error occured</param>
        /// <returns>Result of the cmd</returns>
        public string ExecuteArgs(string cmdName, string[] args, out bool errorOccured)
        {
            errorOccured = false;

            if (args != null && args.Length > 0)
            {
                return Execute(cmdName + " " + args.Aggregate((current, next) => current + " " + next), out errorOccured);
            }

            return "";
        }

        /// <summary>
        /// Executes a program based on program args
        /// Command included
        /// </summary>
        /// <param name="args">1:1 args program parameter</param>
        /// <param name="errorOccured">Out parameter which defines wether an error occured</param>
        /// <returns></returns>
        public string ExecuteArgs(string[] args, out bool errorOccured)
        {
            string result = "";
            errorOccured = false;
            if (args.Length > 0)
            {
                string cmd = args[0];
                string[] parms = null;
                if (args.Length > 1)
                {
                    List<string> temp = args.ToList();
                    temp.RemoveAt(0);
                    parms = temp.ToArray();
                }
                if (cmd.Contains("?") || (parms != null && (parms.Length == 1 && parms[0] == "?")))
                {
                    cmd = "?" + cmd.Replace("?", "");
                    result = CommandShellManager.Singleton.Execute(cmd, out errorOccured);
                }
                else
                {
                    result = CommandShellManager.Singleton.ExecuteArgs(cmd, parms, out errorOccured);
                }
            }
            return result;
        }


        /// <summary>
        /// Execute a shell command
        /// </summary>
        /// <param name="command">Command line content</param>
        /// <param name="errorOccured">Out parameter which defines wether an error occured</param>
        /// <returns>Return-Message of the execute method</returns>
        public string Execute(string command, out bool errorOccured)
        {
            // execute and return
            return ActiveShellContext.Execute(command, out errorOccured);
        }

        /// <summary>
        /// Execute multiple commands
        /// </summary>
        /// <param name="commands">String with commands</param>
        /// <param name="continueOnError">Continues if an error occurse</param>
        /// <param name="errorOccured">Out parameter which defines wether an error occured</param>
        /// <returns>Result-String</returns>
        public string ExecuteMultiline(string commands, bool continueOnError, out bool errorOccured)
        {
            return ActiveShellContext.ExecuteMultiline(commands, continueOnError, out errorOccured);
        }

        /// <summary>
        /// Execute all commands in a file
        /// </summary>
        /// <param name="fileName">Path to the file</param>
        /// <param name="continueOnError">Continues if an error occurse</param>
        /// <param name="errorOccured">Out parameter which defines wether an error occured</param>
        /// <returns>Result-String</returns>
        public string ExecuteFile(string fileName, bool continueOnError, out bool errorOccured)
        { 
            if(System.IO.File.Exists(fileName) == false)
            {
                errorOccured = true;
                return "File " + fileName + " does not exists.";
            }

            return ActiveShellContext.ExecuteMultiline(System.IO.File.ReadAllText(fileName), continueOnError, out errorOccured);
        }

        /// <summary>
        /// Create shell context, if it is the first shell context, it will be activated directly
        /// </summary>
        /// <param name="contextName">Name of the context</param>
        /// <returns>Instance if the shell context</returns>
        public CommandShellContext CreateShellContext(string contextName)
        {
            if (contextName == null)
            {
                throw new ArgumentNullException("contextName");
            }
            if (shellContext.ContainsKey(contextName.ToLower()))
            {
                throw new ArgumentException(message: "Shell-Context already exists.", paramName: "contextName");
            }

            CommandShellContext returnValue = new CommandShellContext(contextName);

            // Add
            shellContext.Add(contextName.ToLower(), returnValue);

            //
            if (activatedShellContext.Count == 0)
            {
                ActivateShellContext(contextName);
            }

            return returnValue;
        }

        /// <summary>
        /// Activate a shell context by its name, if the shell context is already activated, nothing happens.
        /// </summary>
        /// <param name="contextName">Name of the shell context</param>
        public void ActivateShellContext(string contextName)
        {
            if (contextName == null)
            {
                throw new ArgumentNullException("contextName");
            }

            if (shellContext.ContainsKey(contextName.ToLower()))
            {
                CommandShellContext sc = shellContext[contextName.ToLower()];

                if (activatedShellContext.Count > 0)
                {
                    if (activatedShellContext.Peek() != sc)
                    {
                        activatedShellContext.Push(sc);
                    }
                }
                else
                {
                    activatedShellContext.Push(sc);
                }
            }
            else
            {
                throw new ArgumentException(message: "Shell-Context not found.", paramName: "contextName");
            }
        }

        /// <summary>
        /// Move to last shell context. For example for nested contexts and exit commands
        /// </summary>
        public void ActivateLastShellContext()
        {
            if (activatedShellContext.Count > 0)
            {
                activatedShellContext.Pop();
            }
        }

        /// <summary>
        /// Cound the active shell contexts
        /// </summary>
        /// <returns>Number of items</returns>
        public int CoutActivatedShellCotnexts()
        {
            return activatedShellContext.Count;
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Get the active shell context
        /// </summary>
        public CommandShellContext ActiveShellContext
        {
            get
            {
                return activatedShellContext.Peek();
            }
        }
        #endregion
    }
}