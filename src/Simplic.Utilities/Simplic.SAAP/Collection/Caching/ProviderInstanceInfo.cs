using Simplic.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simplic.SAAP
{
    /// <summary>
    /// Stores all provider information
    /// </summary>
    public class ProviderInstanceInfo
    {
        #region Private Member

        private IProvider instance;
        private Dictionary<MethodInfoKey, MethodInfo> methodInfos;
        private Dictionary<string, PropertyInfo> propertyInfos;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create provider instance
        /// </summary>
        /// <param name="instance">Instance of a Provider</param>
        public ProviderInstanceInfo(IProvider instance)
        {
            this.instance = instance;
            methodInfos = new Dictionary<MethodInfoKey, MethodInfo>();
            propertyInfos = new Dictionary<string, PropertyInfo>();

            // Get all methods and properties for caching
            var methods = instance.GetType().GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            //
            List<MethodInfoKey> filterTypes = typeof(object).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
                .Select(Item => new MethodInfoKey(Item.Name, Item.GetParameters().Select(Item2 => Item2.ParameterType).ToArray())).ToList();

            foreach (var method in methods)
            {
                if (method.GetParameters() != null)
                {
                    Type[] types = method.GetParameters().Select(Item => Item.ParameterType).ToArray();

                    var newMethodKey = new MethodInfoKey(method.Name, types);

                    if (filterTypes.Contains(newMethodKey) == false)
                    {
                        methodInfos.Add(newMethodKey, method);
                    }
                }
            }

            // Get all properties
            var properties = instance.GetType().GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            foreach (var prop in properties)
            {
                propertyInfos.Add(prop.Name, prop);
            }
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Call a method with no return value
        /// </summary>
        /// <param name="methodName">Name of the method</param>
        /// <param name="types">List with parameter types</param>
        /// <param name="generictypes">List with generic types</param>
        /// <param name="parameter">List of parameters</param>
        public void CallMethodNoReturn(string methodName, Type[] types, Type[] generictypes, params object[] parameter)
        {
            MethodInfoKey key = new MethodInfoKey(methodName, types);

            if (!methodInfos.ContainsKey(key))
            {
                throw new Exception("Method does not exists: " + methodName);
            }
            else
            {
                MethodInfo info = generictypes == null ? methodInfos[key] : methodInfos[key].MakeGenericMethod(generictypes);

                info.Invoke(instance, parameter);
            }
        }

        /// <summary>
        /// Call a method with an return value
        /// </summary>
        /// <typeparam name="T">Return type of the method</typeparam>
        /// <param name="methodName">Name of the method</param>
        /// <param name="types">List with parameter types</param>
        /// <param name="generictypes">List with generic types</param>
        /// <param name="parameter">List of parameters</param>
        /// <returns>Returnvalue of the method</returns>
        public T CallMethod<T>(string methodName, Type[] types, Type[] generictypes, params object[] parameter)
        {
            T returnValue = default(T);

            MethodInfoKey key = new MethodInfoKey(methodName, types);

            if (!methodInfos.ContainsKey(key))
            {
                throw new Exception("Method does not exists: " + methodName);
            }
            else
            {
                MethodInfo info = generictypes == null ? methodInfos[key] : methodInfos[key].MakeGenericMethod(generictypes);

                returnValue = (T)info.Invoke(instance, parameter);
            }

            return returnValue;
        }

        /// <summary>
        /// Get value of a member
        /// </summary>
        /// <typeparam name="T">Type of the member</typeparam>
        /// <param name="memberName">Name of the member</param>
        /// <param name="index">Index object</param>
        /// <returns>Value of the property</returns>
        public T GetMemberValue<T>(string memberName, object[] index = null)
        {
            T returnValue = default(T);

            if (!propertyInfos.ContainsKey(memberName))
            {
                throw new Exception("Property does not exists: " + memberName);
            }
            else
            {
                returnValue = (T)propertyInfos[memberName].GetValue(instance, index);
            }

            return returnValue;
        }

        /// <summary>
        /// Set value of a member
        /// </summary>
        /// <typeparam name="T">Type of the member</typeparam>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void SetMemberValue<T>(string memberName, T value, object[] index = null)
        {
            if (!propertyInfos.ContainsKey(memberName))
            {
                throw new Exception("Property does not exists: " + memberName);
            }
            else
            {
                propertyInfos[memberName].SetValue(instance, value, index);
            }
        }

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// PlugIn instance
        /// </summary>
        public IProvider Instance
        {
            get { return instance; }
        }

        #endregion Public Member
    }
}