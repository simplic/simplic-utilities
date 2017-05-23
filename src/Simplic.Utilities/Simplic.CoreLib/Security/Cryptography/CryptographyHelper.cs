using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Simplic.Security.Cryptography
{
    /// <summary>
    /// Provide some crypthography helper
    /// </summary>
    public class CryptographyHelper
    {
        #region [Encryption/Decryption]

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="Key">The key.</param>
        /// <param name="IV">The IV.</param>
        /// <returns></returns>
        private static byte[] EncryptString(byte[] clearText, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearText, 0, clearText.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        public static string EncryptString(string clearText, string Password)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = EncryptString(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="cipherData">The cipher data.</param>
        /// <param name="Key">The key.</param>
        /// <param name="IV">The IV.</param>
        /// <returns></returns>
        private static byte[] DecryptString(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        public static string DecryptString(string cipherText, string Password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = DecryptString(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        #endregion [Encryption/Decryption]

        /// <summary>
        /// Create sha256 hash from string (Use UTF-8 encoding)
        /// </summary>
        /// <param name="toHash">String to hash</param>
        /// <returns>Hashed string</returns>
        public static String HashSHA256(string toHash)
        {
            return HashSHA256(toHash, Encoding.UTF8);
        }

        /// <summary>
        /// Create sha256 hash from string
        /// </summary>
        /// <param name="toHash">String to hash</param>
        /// <param name="encoding">String encoding</param>
        /// <returns>Hashed string</returns>
        public static String HashSHA256(string toHash, Encoding encoding)
        {
            StringBuilder returnValue = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Byte[] result = hash.ComputeHash(encoding.GetBytes(toHash));

                foreach (Byte b in result)
                {
                    returnValue.Append(b.ToString("x2"));
                }
            }

            return returnValue.ToString();
        }

        /// <summary>
        /// Create sha256 hash from byte-array
        /// </summary>
        /// <param name="toHash">Byte to hash</param>
        /// <returns>Hashed string</returns>
        public static String HashSHA256(byte[] toHash)
        {
            StringBuilder returnValue = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Byte[] result = hash.ComputeHash(toHash);

                foreach (Byte b in result)
                {
                    returnValue.Append(b.ToString("x2"));
                }
            }

            return returnValue.ToString();
        }

        /// <summary>
        /// Generate MD5-Hash from string (Use UTF-8 encoding)
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Returns MD5-Hash</returns>
        public static string GetMD5Hash(string input)
        {
            return GetMD5Hash(input, Encoding.UTF8);
        }

        /// <summary>
        /// Generate MD5-Hash from string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Returns MD5-Hash</returns>
        public static string GetMD5Hash(string input, Encoding encoding)
        {
            //Prüfen ob Daten übergeben wurden.
            if ((input == null) || (input.Length == 0))
            {
                return string.Empty;
            }

            //MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
            //zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = encoding.GetBytes(input);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result);
        }
    }
}