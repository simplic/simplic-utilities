using System;
using System.Runtime.CompilerServices;

namespace Simplic.SAAP
{
    /// <summary>
    /// Main Provider-Manager. All Manager-Implementations must derive from this class
    /// </summary>
    public abstract class ProviderCollectionController<A> where A : IProvider
    {
        /// <summary>
        /// Create provider collection controller, for implementing in a manager class
        /// </summary>
        public ProviderCollectionController()
        {
            // Use auto-register
            this.RegisterCollection(typeof(A));
        }

        /// <summary>
        /// Get all active providers
        /// </summary>
        /// <typeparam name="T">Type of the provider implementation interface</typeparam>
        /// <returns>Array with active providers</returns>
        public T[] GetActiveProvider<T>() where T : IProvider
        {
            return ProviderHost.Singleton.GetActiveProvider<T>(ProviderCollectionName);
        }

        /// <summary>
        /// Get all active providers
        /// </summary>
        /// <returns>Array with active providers</returns>
        public A[] GetActiveProvider()
        {
            return ProviderHost.Singleton.GetActiveProvider<A>(ProviderCollectionName);
        }

        /// <summary>
        /// Execute a provider method
        /// </summary>
        /// <param name="methodName">Name of the method</param>
        /// <param name="types">List with parameter types</param>
        /// <param name="generictypes">List with generic types</param>
        /// <param name="parameter">List with parameter</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal void CallMethodNoReturn(string methodName, Type[] types, Type[] generictypes, params object[] parameter)
        {
            ProviderHost.Singleton.CallMethodNoReturn(ProviderCollectionName, methodName, types, generictypes, parameter);
        }

        /// <summary>
        /// Execute a provider method and return a value
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="methodName">Name of the method</param>
        /// <param name="types">Parameter type list</param>
        /// <param name="generictypes">Generic type list</param>
        /// <param name="parameter">List with parameter</param>
        /// <returns>Return value as an array, becasue there could be more than one provider called</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal T[] CallMethod<T>(string methodName, Type[] types, Type[] generictypes, params object[] parameter)
        {
            return ProviderHost.Singleton.CallMethod<T>(ProviderCollectionName, methodName, types, generictypes, parameter);
        }

        /// <summary>
        /// Get a value from a member
        /// </summary>
        /// <typeparam name="T">Type of the member</typeparam>
        /// <param name="memberName">Name of the member</param>
        /// <param name="index">Index</param>
        /// <returns>Value of the member</returns>
        internal T[] GetMemberValue<T>(string memberName, object[] index = null)
        {
            return ProviderHost.Singleton.GetMemberValue<T>(ProviderCollectionName, memberName, index);
        }

        /// <summary>
        /// Set a member value
        /// </summary>
        /// <typeparam name="T">Type of the member</typeparam>
        /// <param name="memberName"></param>
        /// <param name="value">Name of the member</param>
        /// <param name="index"></param>
        internal void SetMemberValue<T>(string memberName, T value, object[] index = null)
        {
            ProviderHost.Singleton.SetMemberValue<T>(ProviderCollectionName, memberName, value, index);
        }

        /// <summary>
        /// Register new provider collection
        /// </summary>
        /// <param name="interfaceType">Type of the interface which will be implemented in all providers in this colleciton</param>
        public void RegisterCollection(Type interfaceType)
        {
            ProviderHost.Singleton.RegisterCollection(interfaceType, ProviderCollectionName);
        }

        /// <summary>
        /// Register new provider type which will be added to a provider collection
        /// </summary>
        /// <param name="implementation">Implementation of a Provider</param>
        /// <param name="implementationName">Name of the implementation, it must be unique in a provider collection</param>
        public void RegisterProviderType(Type implementation, string implementationName)
        {
            ProviderHost.Singleton.RegisterProviderType(ProviderCollectionName, implementation, implementationName);
        }

        /// <summary>
        /// Activate a Provider
        /// </summary>
        /// <param name="implementationName">Name of the implementation in the collection</param>
        /// <param name="deactivateOther">Deactivate all other, it is possible to </param>
        /// <param name="getNewIfExists">Force new instance of the Provider</param>
        /// <param name="parameter">Parameter for the init method</param>
        /// <returns>Instance of the Provider</returns>
        public IProvider Activate(string implementationName, bool deactivateOther, bool getNewIfExists, params object[] parameter)
        {
            return ProviderHost.Singleton.Activate(ProviderCollectionName, implementationName, deactivateOther, getNewIfExists, parameter);
        }

        /// <summary>
        /// Activate a Provider
        /// </summary>
        /// <typeparam name="T">Type of the Provider</typeparam>
        /// <param name="implementationName">Name of the implementation in the collection</param>
        /// <param name="deactivateOther">Deactivate all other, it is possible to </param>
        /// <param name="getNewIfExists">Force new instance of the Provider</param>
        /// <param name="parameter">Parameter for the init method</param>
        /// <returns>Instance of the Provider</returns>
        public T Activate<T>(string implementationName, bool deactivateOther, bool getNewIfExists, params object[] parameter) where T : IProvider
        {
            return ProviderHost.Singleton.Activate<T>(ProviderCollectionName, implementationName, deactivateOther, getNewIfExists, parameter);
        }

        /// <summary>
        /// Get new instance of a Provider
        /// </summary>
        /// <typeparam name="T">Type of the Provider</typeparam>
        /// <param name="implementationName">Name of the provider in the provider collection</param>
        /// <param name="parameter">Parameter list</param>
        /// <returns>New instance of a Provider</returns>
        public T GetNewIndependentInstance<T>(string implementationName, params object[] parameter) where T : IProvider
        {
            return ProviderHost.Singleton.GetNewIndependentInstance<T>(ProviderCollectionName, implementationName, parameter);
        }

        /// <summary>
        /// Shutdown the current provider implementation
        /// </summary>
        public void Shutdown()
        {
            ProviderHost.Singleton.Shutdown(ProviderCollectionName);
        }

        /// <summary>
        /// Name of the provider controller, must be unique
        /// </summary>
        public abstract string ProviderCollectionName
        {
            get;
        }
    }
}