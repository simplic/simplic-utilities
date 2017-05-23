using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Simplic.SAAP
{
    /// <summary>
    /// Core of the SAAP. The ProviderHost is the central system for managing the SAAP System
    /// </summary>
    public class ProviderHost
    {
        #region Singleton

        private static readonly ProviderHost singleton = new ProviderHost();

        /// <summary>
        /// Singleton access to the ProviderHost
        /// </summary>
        public static ProviderHost Singleton
        {
            get { return ProviderHost.singleton; }
        }

        #endregion Singleton

        #region Private Member

        private IDictionary<string, ProviderCollection> ProviderCollections;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create new ProviderHost and initialize everything
        /// </summary>
        private ProviderHost()
        {
            ProviderCollections = new Dictionary<string, ProviderCollection>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Get all active providers
        /// </summary>
        /// <typeparam name="T">Type of the provider implementation interface</typeparam>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        /// <returns>Array with active providers</returns>
        public T[] GetActiveProvider<T>(string providerCollectionName) where T : IProvider
        {
            IList<T> returnValue = new List<T>();

            if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];

            foreach (var inst in collection.Instances)
            {
                returnValue.Add((T)inst.Value.Instance);
            }

            return returnValue.ToArray();
        }

        /// <summary>
        /// Execute a provider method
        /// </summary>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="types">List with parameter types</param>
        /// <param name="generictypes">List with generic types</param>
        /// <param name="parameter">List with parameter</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal void CallMethodNoReturn(string providerCollectionName, string methodName, Type[] types, Type[] generictypes, params object[] parameter)
        {
            if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException("methodName must not be null or white space");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];
            collection.CallMethodNoReturn(methodName, types, generictypes, parameter);
        }

        /// <summary>
        /// Execute a provider method and return a value
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="types">List with parameter types</param>
        /// <param name="generictypes">List with generic types</param>
        /// <param name="parameter">List with parameter</param>
        /// <returns>Return value as an array, becasue there could be more than one provider called</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal T[] CallMethod<T>(string providerCollectionName, string methodName, Type[] types, Type[] generictypes, params object[] parameter)
        {
            if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException("methodName must not be null or white space");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];
            return collection.CallMethod<T>(methodName, types, generictypes, parameter);
        }

        /// <summary>
        /// Get a value from a member
        /// </summary>
        /// <typeparam name="T">Type of the member</typeparam>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        /// <param name="memberName">Name of the member</param>
        /// /// <param name="index">Index</param>
        /// <returns>Value of the member</returns>
        internal T[] GetMemberValue<T>(string providerCollectionName, string memberName, object[] index = null)
        {
            if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (string.IsNullOrWhiteSpace(memberName))
            {
                throw new ArgumentException("memberName must not be null or white space");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];
            return collection.GetMemberValue<T>(memberName, index);
        }

        /// <summary>
        /// Set a member value
        /// </summary>
        /// <typeparam name="T">Type of the member</typeparam>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        /// <param name="memberName"></param>
        /// <param name="value">Name of the member</param>
        /// <param name="index"></param>
        internal void SetMemberValue<T>(string providerCollectionName, string memberName, T value, object[] index = null)
        {
            if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (string.IsNullOrWhiteSpace(memberName))
            {
                throw new ArgumentException("memberName must not be null or white space");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];
            collection.SetMemberValue<T>(memberName, value, index);
        }

        /// <summary>
        /// Register new provider collection
        /// </summary>
        /// <param name="interfaceType">Type of the interface which will be implemented in all providers in this colleciton</param>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        public void RegisterCollection(Type interfaceType, string providerCollectionName)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException("interfaceType");
            }
            else if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (interfaceType.IsInterface == false && interfaceType.IsAbstract == false)
            {
                throw new ArgumentException("interfaceType must be an interface or abstract class");
            }
            else if (ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' already exists. Notice, the name must be unique and is case insesitive.");
            }

            ProviderCollection collection = new ProviderCollection(interfaceType, providerCollectionName);
            ProviderCollections.Add(providerCollectionName.ToLower(), collection);
        }

        /// <summary>
        /// Register new provider type which will be added to a provider collection
        /// </summary>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        /// <param name="implementation">Implementation of a Provider</param>
        /// <param name="implementationName">Name of the implementation, it must be unique in a provider collection</param>
        public void RegisterProviderType(string providerCollectionName, Type implementation, string implementationName)
        {
            if (implementation == null)
            {
                throw new ArgumentNullException("implementation");
            }
            else if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (string.IsNullOrWhiteSpace(implementationName))
            {
                throw new ArgumentException("implementationName must not be null or white space");
            }
            else if (!typeof(IProvider).IsAssignableFrom(implementation))
            {
                throw new ArgumentException(message: implementation.FullName + " must implement IProvider");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];
            collection.RegisterProviderType(implementation, implementationName);
        }

        /// <summary>
        /// Activate a Provider
        /// </summary>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        /// <param name="implementationName">Name of the implementation in the collection</param>
        /// <param name="deactivateOther">Deactivate all other, it is possible to </param>
        /// <param name="getNewIfExists">Force new instance of the Provider</param>
        /// <param name="parameter">Parameter for the init method</param>
        /// <returns>Instance of the Provider</returns>
        public IProvider Activate(string providerCollectionName, string implementationName, bool deactivateOther, bool getNewIfExists, params object[] parameter)
        {
            return Activate<IProvider>(providerCollectionName, implementationName, deactivateOther, getNewIfExists, parameter);
        }

        /// <summary>
        /// Activate a Provider
        /// </summary>
        /// <typeparam name="T">Type of the Provider</typeparam>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        /// <param name="implementationName">Name of the implementation in the collection</param>
        /// <param name="deactivateOther">Deactivate all other, it is possible to </param>
        /// <param name="getNewIfExists">Force new instance of the Provider</param>
        /// <param name="parameter">Parameter for the init method</param>
        /// <returns>Instance of the Provider</returns>
        public T Activate<T>(string providerCollectionName, string implementationName, bool deactivateOther, bool getNewIfExists, params object[] parameter) where T : IProvider
        {
            if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (string.IsNullOrWhiteSpace(implementationName))
            {
                throw new ArgumentException("implementationName must not be null or white space");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];

            if (deactivateOther)
            {
                collection.DeactivateAll();
            }

            return collection.GetInstance<T>(implementationName, getNewIfExists, parameter);
        }

        /// <summary>
        /// Get new instance of a Provider
        /// </summary>
        /// <typeparam name="T">Type of the Provider</typeparam>
        /// <param name="providerCollectionName">name of the provider collection</param>
        /// <param name="implementationName">Name of the provider in the provider collection</param>
        /// <param name="parameter">Parameter list</param>
        /// <returns>New instance of a Provider</returns>
        public T GetNewIndependentInstance<T>(string providerCollectionName, string implementationName, params object[] parameter) where T : IProvider
        {
            if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (string.IsNullOrWhiteSpace(implementationName))
            {
                throw new ArgumentException("implementationName must not be null or white space");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];
            return collection.GetNewIndependentInstance<T>(implementationName, parameter);
        }

        /// <summary>
        /// Shutdown all provider implementations
        /// </summary>
        public void Shutdown()
        {
            foreach (var col in ProviderCollections)
            {
                col.Value.Shutdown();
            }
        }

        /// <summary>
        /// Shutdown one provider implementation
        /// </summary>
        /// <param name="providerCollectionName">Name of the collection</param>
        public void Shutdown(string providerCollectionName)
        {
            if (string.IsNullOrWhiteSpace(providerCollectionName))
            {
                throw new ArgumentException("providerCollectionName must not be null or white space");
            }
            else if (!ProviderCollections.ContainsKey(providerCollectionName.ToLower()))
            {
                throw new Exception("A provider collection with the name '" + providerCollectionName + "' does not exists.");
            }

            var collection = ProviderCollections[providerCollectionName.ToLower()];
            collection.Shutdown();
        }

        #endregion Public Methods
    }
}