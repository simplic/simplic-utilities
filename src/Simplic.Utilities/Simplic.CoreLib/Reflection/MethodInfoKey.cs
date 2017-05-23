using System;
using System.Linq;
using System.Text;

namespace Simplic.Reflection
{
    /// <summary>
    /// MethodInfo for caching data
    /// </summary>
    public class MethodInfoKey
    {
        #region Constructor

        /// <summary>
        /// Create new MethodInfoKey for caching information
        /// </summary>
        /// <param name="methodName">Name of the method</param>
        /// <param name="parameter">List of parameter</param>
        public MethodInfoKey(string methodName, params Type[] parameter)
        {
            MethodName = methodName;
            Parameter = parameter.ToArray();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Get the hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(MethodName);

            if (Parameter != null)
            {
                foreach (var param in Parameter)
                {
                    if (param != null)
                    {
                        bool canBeNull = !param.IsValueType || (Nullable.GetUnderlyingType(param) != null);

                        if (canBeNull == false)
                        {
                            builder.Append(param.GetHashCode().ToString());
                        }
                        else
                        {
                            builder.Append("0");
                        }
                    }
                    else
                    {
                        builder.Append("0");
                    }
                }
            }

            return builder.ToString().GetHashCode();
        }

        /// <summary>
        /// Copare MethodInfoKey instances
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the instances has the same HashCode</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj is MethodInfoKey)
            {
                return obj.GetHashCode() == this.GetHashCode();
            }
            else
            {
                return false;
            }
        }

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// Name of the method
        /// </summary>
        public string MethodName
        {
            get;
            private set;
        }

        /// <summary>
        /// Parameter
        /// </summary>
        public Type[] Parameter
        {
            get;
            private set;
        }

        #endregion Public Member
    }
}