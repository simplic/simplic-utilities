using System;
using System.IO;

namespace Simplic.IO
{
    /// <summary>
    /// Stellt Funktionen bereit, die den Umgang mit Dateien im Dateisystem erleichtern​
    /// </summary>
    public class FileHelper
    {
        #region Public Methods

        /// <summary>
        /// Check wether a file has read-access
        /// </summary>
        /// <param name="Path">Path to the file</param>
        /// <returns>True if the file is readable</returns>
        public static bool FileHasReadAccess(string Path)
        {
            try
            {
                System.IO.File.Open(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read).Dispose();
                return true;
            }
            catch (System.IO.IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Überprüft, ob eine Datei Schreibzugriffe hat
        /// </summary>
        /// <param name="Path">Pfad zu der Datei</param>
        /// <returns>True, wenn man Schreibzugriff auf die Datei hat</returns>
        public static bool FileHasWriteAccess(string Path)
        {
            try
            {
                System.IO.File.Open(Path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite).Dispose();
                return true;
            }
            catch (System.IO.IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Versucht eine Datei in ein anderes Verzeichnis zu verschieben und anschließend zu löschen, gibt die Binärdaten der Datei zurück
        /// </summary>
        /// <param name="fileName">Dateiname der einzulesenden Datei</param>
        /// <param name="tmpFolder">Verzeichnis in welches die Datei kopiert wird</param>
        /// <returns>Binärdaten der (erfolgreich) eingelesenen Datei</returns>
        public static byte[] GetFile(string fileName, string tmpFolder)
        {
            byte[] ret_val = null;

            if (tmpFolder.Substring(tmpFolder.Length - 1, 1) != @"\")
            {
                tmpFolder += @"\";
            }

            FileInfo file = new FileInfo(fileName);
            try
            {
                string tmpFileName = Guid.NewGuid().ToString();
                file.MoveTo(tmpFolder + tmpFileName);
                file.Refresh();

                try
                {
                    tmpFileName = file.FullName;
                    ret_val = File.ReadAllBytes(tmpFileName);
                    //
                    if (ret_val != null)
                    {
                        file.Delete();
                    }
                }
                catch
                {
                    //Logging.AddEventLogEntry("Fehler im Job " + this.JobName + ", Datei '" + origFullFileName + "' wird an den Ursprungsort zurückgelegt! " + Logging.GetErrorMsgLog(ex), System.Diagnostics.EventLogEntryType.Warning, true);
                    file.MoveTo(fileName);
                    file.Refresh();
                }
            }
            catch (Exception ex)
            {
                //Logging.AddEventLogEntry("Fehler im Job " + this.JobName + ", Datei '" + origFullFileName + "' wird übersprungen! " + Logging.GetErrorMsgLog(ex), System.Diagnostics.EventLogEntryType.Warning, true);
                throw new Exception("Failure in GetFile", ex);
            }

            return ret_val;
        }

        /// <summary>
        /// Check wether a file is locked
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        #endregion Public Methods
    }
}