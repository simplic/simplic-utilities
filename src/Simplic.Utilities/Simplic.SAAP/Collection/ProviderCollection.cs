using System;
using System.Collections.Generic;

namespace Simplic.SAAP
{
    /// <summary>
    /// Intern provider collection
    /// </summary>
    public class ProviderCollection
    {
        #region Private Member

        private IDictionary<string, Type> implementations;
        private IDictionary<string, ProviderInstanceInfo> instances;
        private IDictionary<string, ProviderInstanceInfo> deactivatedInstances;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Crate new provider collection, which stores all providers
        /// </summary>
        /// <param name="interfaceType">Type of the implementation interface</param>
        /// <param name="providerCollectionName">Name of the provider collection</param>
        public ProviderCollection(Type interfaceType, string providerCollectionName)
        {
            InterfaceType = interfaceType;
            CollectionName = providerCollectionName;

            implementations = new Dictionary<string, Type>();
            instances = new Dictionary<string, ProviderInstanceInfo>();
            deactivatedInstances = new Dictionary<string, ProviderInstanceInfo>();
        }

        #endregion Constructor

        #region Public Methods

        #region [RegisterProviderType]

        /// <summary>
        /// Register new puzzly
        /// </summary>
        /// <param name="implementation">Type of the provider implementation</param>
        /// <param name="implementationName">Unique name of the Provider</param>
        public void RegisterProviderType(Type implementation, string implementationName)
        {
            if (!implementations.ContainsKey(implementationName.ToLower()))
            {
                implementations.Add(implementationName.ToLower(), implementation);
            }
            else
            {
                throw new Exception(implementationName + " already exists in the collection: '" + implementationName + "'");
            }
        }

        #endregion [RegisterProviderType]

        #region [ClearInstances]

        /// <summary>
        /// Clear all instances
        /// </summary>
        public void ClearInstances()
        {
            foreach (var instance in instances)
            {
                instance.Value.Instance.Remove();
            }

            instances.Clear();
        }

        #endregion [ClearInstances]

        #region [DeactivateAll]

        /// <summary>
        /// Deactivate all instances
        /// </summary>
        public void DeactivateAll()
        {
            foreach (var instance in instances)
            {
                if (!deactivatedInstances.ContainsKey(instance.Key.ToLower()))
                {
                    instance.Value.Instance.Deactivate();

                    deactivatedInstances.Add(instance.Key.ToLower(), instance.Value);
                }
            }

            instances.Clear();
        }

        #endregion [DeactivateAll]

        #region [GetNewInstance]

        /// <summary>
        /// Create new instance of a Provider
        /// </summary>
        /// <typeparam name="T">Type of the Provider</typeparam>
        /// <param name="implementationName">Name of the Provider</param>
        /// <param name="parameter">Parameter list for the initialize method</param>
        /// <returns>Instance of the Provider</returns>
        public T GetNewIndependentInstance<T>(string implementationName, params object[] parameter) where T : IProvider
        {
            if (implementations.ContainsKey(implementationName.ToLower()))
            {
                T instance = (T)Activator.CreateInstance(implementations[implementationName.ToLower()]);
                instance.Initialize(parameter);
                instance.Activate();

                return instance;
            }
            else
            {
                throw new Exception("The provider with the name '" + implementationName + "'does not exists in the provider collection '" + CollectionName + "'");
            }
        }

        #endregion [GetNewInstance]

        #region [GetInstance]

        /// <summary>
        /// Get the instance of a Provider
        /// </summary>
        /// <typeparam name="T">Type of the Provider</typeparam>
        /// <param name="implementationName">Name of the provider in the collection</param>
        /// <param name="removeIfExists">Remove all existing providers from the given type</param>
        /// <param name="parameter">Parameter of the initialize method</param>
        /// <returns>Instance of a Provider</returns>
        public T GetInstance<T>(string implementationName, bool removeIfExists, params object[] parameter) where T : IProvider
        {
            if (removeIfExists)
            {
                if (deactivatedInstances.ContainsKey(implementationName.ToLower()))
                {
                    deactivatedInstances[implementationName.ToLower()].Instance.Deactivate();
                    deactivatedInstances.Remove(implementationName.ToLower());
                }

                if (instances.ContainsKey(implementationName.ToLower()))
                {
                    instances[implementationName.ToLower()].Instance.Remove();
                    instances.Remove(implementationName.ToLower());
                }
            }

            if (deactivatedInstances.ContainsKey(implementationName.ToLower()))
            {
                T instance = (T)deactivatedInstances[implementationName.ToLower()].Instance;
                instance.Activate();
                return instance;
            }
            else if (instances.ContainsKey(implementationName.ToLower()))
            {
                return (T)instances[implementationName.ToLower()].Instance;
            }
            else
            {
                T instance = GetNewIndependentInstance<T>(implementationName, parameter);
                instances.Add(implementationName.ToLower(), new ProviderInstanceInfo(instance));

                new ProviderInstanceInfo(instance);

                return instance;
            }
        }

        #endregion [GetInstance]

        #region [CallMethod]

        /// <summary>
        /// Call a method with no return value
        /// </summary>
        /// <param name="methodName">Name of the method</param>
        /// <param name="types">List of parameter types</param>
        /// <param name="generictypes">List with generic types</param>
        /// <param name="parameter">List of parameters</param>
        internal void CallMethodNoReturn(string methodName, Type[] types, Type[] generictypes, params object[] parameter)
        {
            foreach (var instance in instances)
            {
                instance.Value.CallMethodNoReturn(methodName, types, generictypes, parameter);
            }
        }

        /// <summary>
        /// Call a method with an return value
        /// </summary>
        /// <typeparam name="T">Return type of the method</typeparam>
        /// <param name="methodName">Name of the method</param>
        /// <param name="types">List of parameter types</param>
        /// <param name="generictypes">List with general type</param>
        /// <param name="parameter">List of parameters</param>
        /// <returns>List of returnvalue of the method</returns>
        internal T[] CallMethod<T>(string methodName, Type[] types, Type[] generictypes, params object[] parameter)
        {
            List<T> returnValue = new List<T>();

            foreach (var instance in instances)
            {
                returnValue.Add(instance.Value.CallMethod<T>(methodName, types, generictypes, parameter));
            }

            return returnValue.ToArray();
        }

        #endregion [CallMethod]

        #region [GetMemberValue]

        /// <summary>
        /// Get value of a member
        /// </summary>
        /// <typeparam name="T">Type of the member</typeparam>
        /// <param name="memberName">Name of the member</param>
        /// <param name="index">Index object</param>
        /// <returns>list of values of the property</returns>
        internal T[] GetMemberValue<T>(string memberName, object[] index = null)
        {
            List<T> returnValue = new List<T>();

            foreach (var instance in instances)
            {
                returnValue.Add(instance.Value.GetMemberValue<T>(memberName, index));
            }

            return returnValue.ToArray();
        }

        #endregion [GetMemberValue]

        #region [SetMemberValue]

        /// <summary>
        /// Set value of a member
        /// </summary>
        /// <typeparam name="T">Type of the member</typeparam>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        internal void SetMemberValue<T>(string memberName, T value, object[] index = null)
        {
            foreach (var instance in instances)
            {
                instance.Value.SetMemberValue<T>(memberName, value, index);
            }
        }

        #endregion [SetMemberValue]

        #region [Shutdown]

        /// <summary>
        /// Shutdown all instances
        /// </summary>
        public void Shutdown()
        {
            foreach (var instance in instances)
            {
                instance.Value.Instance.Shutdown();
            }

            instances.Clear();

            foreach (var deactivated in deactivatedInstances)
            {
                deactivated.Value.Instance.Shutdown();
            }

            deactivatedInstances.Clear();
        }

        #endregion [Shutdown]

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// Interface type
        /// </summary>
        public Type InterfaceType
        {
            get;
            private set;
        }

        /// <summary>
        /// Collection name
        /// </summary>
        public string CollectionName
        {
            get;
            private set;
        }

        /// <summary>
        /// Dictionary withj all acitve instances (Get-ONLY)
        /// </summary>
        public IDictionary<string, ProviderInstanceInfo> Instances
        {
            get { return instances; }
        }

        #endregion Public Member
    }
}