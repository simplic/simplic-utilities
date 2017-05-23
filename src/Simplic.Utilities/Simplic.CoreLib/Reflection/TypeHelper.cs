using System;
using System.Collections.Generic;
using System.Reflection;

namespace Simplic.Reflection
{
    /// <summary>
    /// Help to find types
    /// </summary>
    public class TypeHelper
    {
        private static Dictionary<string, Type> typeCache = new Dictionary<string, Type>();

        /// <summary>
        /// Find type in all assemblies
        /// </summary>
        /// <param name="typeName">Full typename / Namespace.Class</param>
        /// <returns>Null if no type was found, else type</returns>
        public static Type FindType(string typeName)
        {
            Type detectedType = null;

            lock (typeCache)
            {
                if (!typeCache.TryGetValue(typeName, out detectedType))
                {
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        detectedType = a.GetType(typeName);
                        if (detectedType != null)
                        {
                            break;
                        }
                    }

                    if (detectedType != null)
                    {
                        typeCache[typeName] = detectedType;
                    }
                }
            }

            return detectedType;
        }
    }
}