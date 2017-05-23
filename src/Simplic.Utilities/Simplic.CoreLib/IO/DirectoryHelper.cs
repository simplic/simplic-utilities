using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.IO
{
    /// <summary>
    /// Provides methods for working with directories. Such as create if not exists and other helpful stuff.
    /// </summary>
    public class DirectoryHelper
    {
        /// <summary>
        /// Creates a directory at the given path, if it's not existing
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <returns>True if the directory already exists</returns>
        public static bool CreateDirectoryIfNotExists(string Path)
        {
            bool returnValue = false;

            try
            {
                string directoryPath = System.IO.Path.GetDirectoryName(Path);

                if (System.IO.Directory.Exists(directoryPath))
                {
                    returnValue = true;
                }
                else
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Der Pfad konnte nicht überprüft werden oder der Ordner konnte nicht angelegt werden.", ex);
            }

            return returnValue;
        }

        /// <summary>
        /// Get all filenames in a directory, without path information
        /// </summary>
        /// <param name="path">Path to the directory</param>
        /// <param name="fileNamesWithoutExtension">If set to true, the filenames will be returned without extensions</param>
        /// <returns>List of filenames in the directory</returns>
        public static string[] GetFileNames(string path, bool fileNamesWithoutExtension = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentOutOfRangeException("path");
            }

            IList<string> files = new List<string>();

            foreach (var file in System.IO.Directory.GetFiles(path))
            {
                if (!fileNamesWithoutExtension)
                {
                    files.Add(System.IO.Path.GetFileName(file));
                }
                else
                {
                    files.Add(System.IO.Path.GetFileNameWithoutExtension(file));
                }
            }

            return files.ToArray();
        }
    }
}