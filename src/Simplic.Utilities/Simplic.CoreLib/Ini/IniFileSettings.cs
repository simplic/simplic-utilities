using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Simplic.Configuration.Ini
{
    /// <summary>
    /// Contains all functions to work with ini files
    /// </summary>
    public class IniFileSettings
    {
        #region [P/Invoke]

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        #endregion [P/Invoke]

        #region Private Member

        private string filePath;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create Ini-File-Reader
        /// </summary>
        /// <param name="filePath">Path to the ini-file</param>
        public IniFileSettings(string filePath)
        {
            this.filePath = filePath;

            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.WriteAllText(filePath, "");
            }
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Read all sections from the ini-file
        /// </summary>
        /// <returns></returns>
        public string[] ReadSections()
        {
            IList<string> sections = new List<string>();

            string content = System.IO.File.ReadAllText(this.filePath);

            if (content != null)
            {
                string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        sections.Add(line.Replace("[", "").Replace("]", ""));
                    }
                }
            }

            return sections.ToArray();
        }

        /// <summary>
        /// Write the settings to the ini file in a section
        /// </summary>
        /// <PARAM name="key">Unique key per section</param>
        /// <param name="value">Setting-Value</param>
        /// <param name="section">Section name</param>
        public void WriteValue<T>(string key, T value, string section = "Default")
        {
            WritePrivateProfileString(section, key, value.ToString(), this.filePath);
        }

        /// <summary>
        /// Read the settings from the ini file in a section
        /// </summary>
        /// <param name="section">Section name</param>
        /// <PARAM name="key">Unique key per section</param>
        /// <param name="defaultValue">Setting-Default-Value; Will be returnde if no value found</param>
        /// <returns>Value</returns>
        public T ReadValue<T>(string key, string section = "Default", T defaultValue = default(T))
        {
            StringBuilder temp = new StringBuilder(255);

            string defvalue = "";

            if (defaultValue != null)
            {
                defvalue = defaultValue.ToString();
            }

            int i = GetPrivateProfileString(section, key, defvalue, temp, 255, this.filePath);

            if (typeof(T) == typeof(int))
            {
                return (T)(object)int.Parse(temp.ToString());
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)float.Parse(temp.ToString());
            }
            else if (typeof(T) == typeof(double))
            {
                return (T)(object)double.Parse(temp.ToString());
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)temp.ToString();
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)bool.Parse(temp.ToString());
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)long.Parse(temp.ToString());
            }
            else if (typeof(T) == typeof(ulong))
            {
                return (T)(object)ulong.Parse(temp.ToString());
            }
            else
            {
                throw new Exception("Generic type: " + typeof(T).ToString() + " is not allowed. The use generic type must be one of the following: string/int/float/double/bool.");
            }
        }

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// Get or set the path to the ini-file
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        #endregion Public Member
    }
}