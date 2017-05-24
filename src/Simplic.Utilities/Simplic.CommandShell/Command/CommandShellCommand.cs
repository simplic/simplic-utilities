using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simplic.CommandShell
{
    /// <summary>
    /// Delegate which must be used to register shell methods
    /// </summary>
    /// <param name="commandName">Name of the command</param>
    /// <param name="parameter">array with parameter</param>
    /// <returns>Result of the command</returns>
    public delegate string CommandShellCommandDelegate(string commandName, CommandShellParameterCollection parameter);

    /// <summary>
    /// Single shell command
    /// </summary>
    public class CommandShellCommand
    {
        #region Private Member

        #endregion

        #region Constructor
        /// <summary>
        /// Init everything
        /// </summary>        
        private CommandShellCommand()
        {
            ClassInstance = null;
        }

        /// <summary>
        /// Create shell command
        /// </summary>
        /// <param name="commandName">name of the command</param>
        /// <param name="methodDelegate">Delegate to the command</param>
        /// <param name="requiredParameter">Reqwuired parameter for the command</param>
        public CommandShellCommand(string commandName, CommandShellCommandDelegate methodDelegate, string[] requiredParameter)
            : this()
        {
            if (commandName == null)
            {
                throw new ArgumentNullException("CommandName");
            }
            if (methodDelegate == null)
            {
                throw new ArgumentNullException("methodDelegate");
            }

            this.CommandName = commandName;
            this.MethodDelegate = methodDelegate;
            this.RequiredParameter = requiredParameter;
        }

        /// <summary>
        /// Create shell command
        /// </summary>
        /// <param name="commandName">name of the command</param>
        /// <param name="classType">Type of the class</param>
        /// <param name="methodName">Name of the method to invoke</param>
        /// <param name="requiredParameter">Reqwuired parameter for the command</param>
        public CommandShellCommand(string commandName, Type classType, string methodName, string[] requiredParameter)
            : this()
        {
            if (commandName == null)
            {
                throw new ArgumentNullException("CommandName");
            }
            if (classType == null)
            {
                throw new ArgumentNullException("className");
            }
            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }

            this.CommandName = commandName;
            this.ClassType = classType;
            this.MethodName = methodName;
            this.RequiredParameter = requiredParameter;
        }

        /// <summary>
        /// Create shell command
        /// </summary>
        /// <param name="commandName">name of the command</param>
        /// <param name="classInstance">Type of the class</param>
        /// <param name="methodName">Name of the method to invoke</param>
        /// <param name="requiredParameter">Reqwuired parameter for the command</param>
        public CommandShellCommand(string commandName, object classInstance, string methodName, string[] requiredParameter)
            : this()
        {
            if (commandName == null)
            {
                throw new ArgumentNullException("CommandName");
            }
            if (classInstance == null)
            {
                throw new ArgumentNullException("classInstance");
            }
            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }

            this.CommandName = commandName;
            this.ClassType = classInstance.GetType();
            this.ClassInstance = classInstance;
            this.MethodName = methodName;
            this.RequiredParameter = requiredParameter;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the help text (command) for the current command
        /// </summary>
        /// <returns>Help text as a string</returns>
        public string GetHelpText()
        {
            StringBuilder returnValue = new StringBuilder();
            CommandDescriptionAttribute commandDesc = null;

            MethodInfo methodInfo = null;

            if (MethodDelegate != null)
            {
                methodInfo = MethodDelegate.Method;
            }
            else if (ClassType != null)
            {
                methodInfo = ClassType.GetMethod(MethodName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            }

            foreach (var atr in methodInfo.GetCustomAttributes(false))
            {
                if (atr is CommandDescriptionAttribute)
                {
                    commandDesc = (atr as CommandDescriptionAttribute);
                }
            }

            returnValue.Append(CommandName);
            if (commandDesc != null)
            {
                returnValue.Append(" " + commandDesc.Description);
            }

            return returnValue.ToString();
        }

        /// <summary>
        /// Get the help text (parameter) for the current command
        /// </summary>
        /// <returns>Help text as a string</returns>
        public string GetMethodHelpText()
        {
            StringBuilder returnValue = new StringBuilder();
            CommandDescriptionAttribute commandDesc = null;
            List<ParameterDescriptionAttribute> paramDescs = new List<ParameterDescriptionAttribute>();

            MethodInfo methodInfo = null;

            if (MethodDelegate != null)
            {
                methodInfo = MethodDelegate.Method;
            }
            else if (ClassType != null)
            {
                methodInfo = ClassType.GetMethod(MethodName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            }

            foreach (var atr in methodInfo.GetCustomAttributes(false))
            {
                if (atr is CommandDescriptionAttribute)
                {
                    commandDesc = (atr as CommandDescriptionAttribute);
                }
                else if (atr is ParameterDescriptionAttribute)
                {
                    paramDescs.Add(atr as ParameterDescriptionAttribute);
                }
            }

            returnValue.AppendLine("");
            if (commandDesc != null)
            {
                returnValue.AppendLine(commandDesc.Description);
            }
            returnValue.AppendLine("");

            returnValue.Append(CommandName.ToUpper() + " ");

            foreach (var paramDesc in paramDescs.OrderBy(Item => !Item.IsRequired).ThenBy(Item => Item.ParameterName))
            {
                if (!paramDesc.IsRequired) { returnValue.Append("["); }
                returnValue.Append("--" + paramDesc.ParameterName);
                if (!paramDesc.IsRequired) { returnValue.Append("]"); }
                returnValue.Append(" ");
            }
            returnValue.AppendLine("");

            if (paramDescs.Count > 0)
            {
                int longestParameter = paramDescs.Max(Item => Item.ParameterName.Length);

                returnValue.AppendLine("");

                foreach (var paramDesc in paramDescs)
                {
                    string placeHolder = new string(' ', (longestParameter - paramDesc.ParameterName.Length) + 2);
                    returnValue.AppendLine(" --" + paramDesc.ParameterName + placeHolder + paramDesc.Description);
                }
            }

            return returnValue.ToString();
        }

        /// <summary>
        /// Execute the current command
        /// </summary>
        /// <param name="parameter">Parameter</param>
        /// <returns>result of the command</returns>
        public string Execute(CommandShellParameter[] parameter)
        {
            CommandShellParameterCollection collection = new CommandShellParameterCollection();

            if (parameter != null)
            {
                foreach (var param in parameter)
                {
                    collection.AddParameter(param);
                }
            }

            if (MethodDelegate != null)
            {
                // call .net method and get the result
                return MethodDelegate.Invoke(this.CommandName, collection);
            }
            else
            {
                IList<object> passParameter = new List<object>();

                //Command is always the first parameter
                passParameter.Add(this.CommandName);

                if (parameter != null && RequiredParameter != null)
                {
                    foreach (string reqParam in RequiredParameter)
                    {
                        foreach (var param in parameter)
                        {
                            if (reqParam.ToLower() == param.Name.ToLower())
                            {
                                passParameter.Add(param.Value);
                            }
                        }
                    }
                }

                var method = ClassType.GetMethod(MethodName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

                if (method != null)
                {
                    return method.Invoke(ClassInstance, passParameter.ToArray()).ToString();
                }
                else
                {
                    return "Command could not be executed!";
                }
            }
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Name of the command
        /// </summary>
        public string CommandName
        {
            get;
            private set;
        }

        /// <summary>
        /// Delegate to the method which will be executed with the command
        /// </summary>
        public CommandShellCommandDelegate MethodDelegate
        {
            get;
            private set;
        }

        /// <summary>
        /// Array of required parameter
        /// </summary>
        public string[] RequiredParameter
        {
            get;
            private set;
        }

        /// <summary>
        /// Name of the method which will be invoked
        /// </summary>
        public string MethodName
        {
            get;
            set;
        }

        /// <summary>
        /// Type of the class in which the method will be invoked
        /// </summary>
        public Type ClassType
        {
            get;
            set;
        }

        /// <summary>
        /// Instance of a class
        /// </summary>
        public object ClassInstance
        {
            get;
            set;
        }
        #endregion
    }
}
