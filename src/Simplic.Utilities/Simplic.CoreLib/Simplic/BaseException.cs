using System;

namespace Simplic
{
    /// <summary>
    /// Base Exception class
    /// </summary>
    public class BaseException : Exception
    {
        private int errorCode;

        /// <summary>
        /// Create exception with simple message and the default error code 0
        /// </summary>
        /// <param name="message">Message text</param>
        public BaseException(string message)
            : base(message)
        {
            errorCode = 0;
        }

        /// <summary>
        /// Create exception with simple message and a specific error code
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="message">Message text</param>
        public BaseException(int errorCode, string message)
            : base(message)
        {
            this.errorCode = errorCode;
        }

        /// <summary>
        /// Create exception with simple message and a specific error code
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="message">Message text</param>
        /// <param name="innerException">Inner exception</param>
        public BaseException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.errorCode = errorCode;
        }

        /// <summary>
        /// Error code
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return errorCode;
            }
        }
    }
}