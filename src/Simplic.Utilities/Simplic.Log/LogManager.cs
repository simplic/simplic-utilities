using Simplic.SAAP;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Simplic.Log
{
    /// <summary>
    /// Logging Manager is the root of the logging system. All actions must be executed over this class.
    /// </summary>
    public class LogManager : ProviderCollectionController<ILogOutput>
    {
        #region [Delegate and events]

        public delegate void LogWriteTrigger(object sender, LogTriggerEventArgs e);

        /// <summary>
        /// Will be called before a log is written. Here you can handle some logType changes and so on
        /// </summary>
        public event LogWriteTrigger OnWriteLog;

        #endregion [Delegate and events]

        #region Private Member

        private IDictionary<string, string> constantParameter;
        private IDictionary<LogType, string> messageFormats;
        private IDictionary<string, DebugInformation> debugAreas;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create new Log-Manager
        /// </summary>
        public LogManager()
        {
            constantParameter = new Dictionary<string, string>();
            debugAreas = new Dictionary<string, DebugInformation>();
            messageFormats = new Dictionary<LogType, string>();

            // Add message formats
            messageFormats.Add(LogType.Debug, "[message]");
            messageFormats.Add(LogType.Info, "[message]");
            messageFormats.Add(LogType.Warning, "[message]");
            messageFormats.Add(LogType.Error, "[message]\r\n[exception]\r\n[innerException]");
            // Add default debug-area
            debugAreas.Add("debug", new DebugInformation() { AreaName = "debug", IsActive = false, UseStopwatch = false });
        }

        #endregion Constructor

        #region Public Methods

        #region [SetMessageConstant]

        /// <summary>
        /// Set a constant which will appear in every method
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="parameterValue">Value of the parameter</param>
        public void SetMessageConstant(string parameterName, string parameterValue)
        {
            parameterName = ToInternConstantName(parameterName);

            if (constantParameter.ContainsKey(parameterName))
            {
                constantParameter[parameterName] = parameterValue;
            }
            else
            {
                constantParameter.Add(parameterName, parameterValue);
            }
        }

        #endregion [SetMessageConstant]

        #region [GetMessageConstant]

        /// <summary>
        /// Return the value of a constnat if exists, else return an empty string
        /// </summary>
        /// <param name="constantName">Name of the constant</param>
        /// <returns>Constant-Content or an empty string</returns>
        public string GetMessageConstant(string constantName)
        {
            constantName = ToInternConstantName(constantName);

            if (constantParameter.ContainsKey(constantName))
            {
                return constantParameter[constantName];
            }
            else
            {
                return "";
            }
        }

        #endregion [GetMessageConstant]

        #region [RemoveMessageConstant]

        /// <summary>
        /// Remove the constant parameter
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        public void RemoveMessageConstant(string parameterName)
        {
            parameterName = ToInternConstantName(parameterName);

            if (constantParameter.ContainsKey(parameterName))
            {
                constantParameter.Remove(parameterName);
            }
        }

        #endregion [RemoveMessageConstant]

        #region [SetMessageFormat]

        /// <summary>
        /// Set the message-format for a logType.
        /// </summary>
        /// <param name="logType">Log type</param>
        /// <param name="messageFormat">Message format string. The message must be formatted liek this: Your message [parameterName]. Example
        /// [timestamp] [logType] - [message]
        /// Predefines consts:
        /// [message]
        /// [timestamp]
        /// [threadId]
        /// [threadName]
        /// [message]
        /// [exception]
        /// [innerException]
        /// [osMachineName]
        /// [osUserName]
        /// [osVersion]
        /// [osArchitecture]
        /// [osDomain]
        /// [memoryInformation]
        /// [processName]
        /// [cmdLineArgs]
        /// </param>
        public void SetMessageFormat(string messageFormat, params LogType[] logTypes)
        {
            if (logTypes != null)
            {
                foreach (var lt in logTypes)
                {
                    if (messageFormats.ContainsKey(lt))
                    {
                        messageFormats[lt] = messageFormat;
                    }
                    else
                    {
                        messageFormats.Add(lt, messageFormat);
                    }
                }
            }
            else
            {
                foreach (var msg in messageFormats)
                {
                    messageFormats[msg.Key] = messageFormat;
                }
            }
        }

        #endregion [SetMessageFormat]

        #region [ActivateDebugArea]

        /// <summary>
        /// Activate a list of debug areas
        /// </summary>
        /// <param name="areaNames">List of debug areas</param>
        public void ActivateDebugArea(params DebugInformation[] areaNames)
        {
            if (areaNames != null)
            {
                foreach (var area in areaNames)
                {
                    if (debugAreas.ContainsKey(area.AreaName.ToLower()))
                    {
                        debugAreas[area.AreaName.ToLower()] = area;
                    }
                    else
                    {
                        debugAreas.Add(area.AreaName.ToLower(), area);
                    }
                }
            }
        }

        #endregion [ActivateDebugArea]

        #region [ActivateDebugArea]

        /// <summary>
        /// deactivate a list of debug areas
        /// </summary>
        /// <param name="areaNames">List of debug areas</param>
        public void DeactivateDebugArea(params string[] areaNames)
        {
            if (areaNames != null)
            {
                foreach (var area in areaNames)
                {
                    if (debugAreas.ContainsKey(area.ToLower()))
                    {
                        debugAreas[area.ToLower()].IsActive = false;
                    }
                    else
                    {
                        debugAreas.Add(area.ToLower(), new DebugInformation() { AreaName = area, IsActive = false });
                    }
                }
            }
        }

        #endregion [ActivateDebugArea]

        #region [DebugAreaIsActive]

        /// <summary>
        /// Proof wether a debug area is active. If the debug area does not exists, the result will be true
        /// </summary>
        /// <param name="debugAreaName">Name of the debug area</param>
        /// <returns>Return the debug activate state</returns>
        public bool DebugAreaIsActive(string debugAreaName)
        {
            if (debugAreas.ContainsKey(debugAreaName.ToLower()))
            {
                return debugAreas[debugAreaName.ToLower()].IsActive;
            }
            else
            {
                return false;
            }
        }

        #endregion [DebugAreaIsActive]

        #region [Write]

        /// <summary>
        /// Write a message to all active log Providers
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="ex">Exception instance</param>
        /// <param name="logType">Type of the message</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="messageFormat">Overrides the default message format for the current message</param>
        public void Write(string message, Exception ex, LogType logType, dynamic parameter, string messageFormat = null, string logArea = "")
        {
            IDictionary<string, object> parameterList = new Dictionary<string, object>();

            if (parameter != null)
            {
                // get all properties
                var props = parameter.GetType().GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

                foreach (PropertyInfo pi in props)
                {
                    // get the value from the property
                    var value = pi.GetValue(parameter, null);

                    if (value != null)
                    {
                        parameterList.Add("[" + pi.Name + "]", value);
                    }
                }
            }

            foreach (var constParam in constantParameter)
            {
                if (!parameterList.ContainsKey(constParam.Key))
                {
                    parameterList.Add(constParam.Key, constParam.Value);
                }
            }

            if (OnWriteLog != null)
            {
                LogTriggerEventArgs e = new LogTriggerEventArgs();
                e.Message = message;
                e.NewLogType = logType;
                e.OldLogType = logType;
                e.ParameterList = parameterList;

                OnWriteLog(this, e);

                logType = e.NewLogType;
            }

            string msg = "";
            if (string.IsNullOrWhiteSpace(messageFormat))
            {
                msg = messageFormats[logType];
            }
            else
            {
                msg = messageFormat;
            }

            // Replace all parameter
            msg = msg.Replace("[message]", message);
            msg = msg.Replace("[timestamp]", DateTime.Now.ToString());
            msg = msg.Replace("[threadId]", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            msg = msg.Replace("[threadName]", System.Threading.Thread.CurrentThread.Name ?? "");

            if (ex != null)
            {
                msg = msg.Replace("[exception]", string.Format("Exception[{0}]: {1}", ex.GetType().Name, ex.ToString()));

                var nextInner = ex.InnerException;
                StringBuilder innerException = new StringBuilder();
                while (nextInner != null)
                {
                    innerException.AppendLine(string.Format("Innerexception[{0}]:", nextInner.GetType().Name));
                    innerException.AppendLine(nextInner.ToString());
                    nextInner = nextInner.InnerException;
                }
                msg = msg.Replace("[innerException]", innerException.ToString());
            }

            msg = msg.Replace("[osMachineName]", Environment.MachineName);
            msg = msg.Replace("[osUserName]", Environment.UserName);
            msg = msg.Replace("[osVersion]", Environment.OSVersion.ToString());
            msg = msg.Replace("[osArchitecture]", Environment.Is64BitProcess == true ? "x64" : "x86");
            msg = msg.Replace("[osDomain]", Environment.UserDomainName);
            msg = msg.Replace("[memoryInformation]", System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            msg = msg.Replace("[processName]", System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            msg = msg.Replace("[logType]", logType.ToString());

            if (Environment.GetCommandLineArgs() != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string line in Environment.GetCommandLineArgs())
                {
                    sb.AppendLine(line);
                }

                msg = msg.Replace("[cmdLineArgs]", sb.ToString());
            }

            // Replace parameter
            foreach (var param in parameterList)
            {
                msg = msg.Replace(param.Key, param.Value?.ToString() ?? "");
            }

            // call the write method in all output implementations
            foreach (ILogOutput output in GetActiveProvider())
            {
                output.Write(msg, logArea, ex, logType, parameterList);
            }
        }

        #endregion [Write]

        #region [Debug]

        /// <summary>
        /// Reset the stopwatch for an given debug area. This can be useful, if you want to reset the watch before a specifig action
        /// </summary>
        /// <param name="debugArea">Name of the debug area</param>
        public void ResetDebugStopwatch(string debugArea = "default")
        {
            if (DebugAreaIsActive(debugArea))
            {
                var area = debugAreas[debugArea.ToLower()];

                // Create new stopwatch
                if (area.UseStopwatch)
                {
                    if (area.Stopwatch == null)
                    {
                        // Create if needed
                        area.Stopwatch = new System.Diagnostics.Stopwatch();
                        area.Stopwatch.Start();
                    }
                    else
                    {
                        // Restart if already existing
                        area.Stopwatch.Restart();
                    }
                }
            }
        }

        /// <summary>
        /// Write debug message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="ex">Exception instance</param>
        /// <param name="logType">Type of the message</param>
        /// <param name="debugArea">Debug area to activate or deactivate debug messages</param>
        /// <param name="messageFormat">Overrides the default message format for the current message</param>
        public void Debug(string message, Exception ex = null, dynamic parameter = null, string debugArea = "default", string messageFormat = null)
        {
            if (DebugAreaIsActive(debugArea))
            {
                var area = debugAreas[debugArea.ToLower()];

                // Create new stopwatch
                if (area.UseStopwatch)
                {
                    if (area.Stopwatch == null)
                    {
                        area.Stopwatch = new System.Diagnostics.Stopwatch();
                        area.Stopwatch.Start();
                    }
                    else
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("Delta-Milliseconds: " + area.Stopwatch.ElapsedMilliseconds.ToString());
                        builder.Append(message);
                        message = builder.ToString();

                        // Restart, because this stopwatch should always be restarted before next call
                        area.Stopwatch.Restart();
                    }
                }

                Write(message, ex, LogType.Debug, parameter, messageFormat, logArea: debugArea.ToLower());
            }
        }

        #endregion [Debug]

        #region [Info]

        /// <summary>
        /// Write information message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="ex">Exception instance</param>
        /// <param name="messageFormat">Overrides the default message format for the current message</param>
        public void Info(string message, Exception ex = null, dynamic parameter = null, string messageFormat = null)
        {
            Write(message, ex, LogType.Info, parameter, messageFormat);
        }

        #endregion [Info]

        #region [Warning]

        /// <summary>
        /// Write warning message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="ex">Exception instance</param>
        /// <param name="messageFormat">Overrides the default message format for the current message</param>
        public void Warning(string message, Exception ex = null, dynamic parameter = null, string messageFormat = null)
        {
            Write(message, ex, LogType.Warning, parameter, messageFormat);
        }

        #endregion [Warning]

        #region [Error]

        /// <summary>
        /// Write error message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="ex">Exception instance</param>
        /// <param name="messageFormat">Overrides the default message format for the current message</param>
        public void Error(string message, Exception ex = null, dynamic parameter = null, string messageFormat = null)
        {
            Write(message, ex, LogType.Error, parameter, messageFormat);
        }

        #endregion [Error]

        #endregion Public Methods

        #region private methods

        /// <summary>
        /// Formats a constant name to the constant name used to access the constant values (dictionary)
        /// </summary>
        /// <param name="constantName"></param>
        /// <returns></returns>
        private string ToInternConstantName(string constantName)
        {
            string ret = constantName.Trim();
            if (!ret.StartsWith("["))
            {
                ret = "[" + ret;
            }
            if (!ret.EndsWith("]"))
            {
                ret = ret + "]";
            }
            return ret;
        }

        #endregion private methods

        #region Public Member

        /// <summary>
        /// Provider name
        /// </summary>
        public override string ProviderCollectionName
        {
            get { return "SimplicLogManager"; }
        }

        #endregion Public Member
    }
}