using System;

namespace Simplic
{
    /// <summary>
    /// Provides function to work with the windows console
    /// </summary>
    public class ConsoleHelper
    {
        /// <summary>
        /// Read password from console input
        /// </summary>
        /// <returns>Password as string</returns>
        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        /// <summary>
        /// Write some thing to the console, if UI is enabled
        /// </summary>
        /// <param name="line">To write to the console</param>
        /// <param name="args">Argument</param>
        public static void WriteLineNoUI(string line, params object[] args)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine(format: line, arg: args);
            }
        }

        public static void PrintRowDelimiter()
        {
            Console.WriteLine(new string('-', Console.BufferWidth - 1));
        }

        public static void PrintRow(params string[] columns)
        {
            int width = ((Console.BufferWidth - 1) - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        private static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}