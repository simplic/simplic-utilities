using System;

namespace Simplic.SAAP
{
    /// <summary>
    /// Provider controller, for writing provider based systems
    /// </summary>
    public class SimpleProviderController<T> where T : IProvider
    {
        #region Private Member

        private EmptyProviderCollectionController<T> controller;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create provider controller
        /// </summary>
        /// <param name="name">Constroller name</param>
        public SimpleProviderController(string name)
        {
            this.controller = new EmptyProviderCollectionController<T>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Add a provider to the controller
        /// </summary>
        /// <param name="providerType">Type of the controller</param>
        /// <param name="providerName">Name of the provider</param>
        public void AddProvider(Type providerType, string providerName)
        {
            controller.RegisterProviderType(providerType, providerName);
        }

        /// <summary>
        /// Get new Provider instance
        /// </summary>
        /// <param name="providerName">Name of the provider</param>
        /// <param name="parameter">Parameter list</param>
        /// <returns>Provider isntance</returns>
        public T GetNewInstance(string providerName, params object[] parameter)
        {
            return controller.GetNewIndependentInstance<T>(providerName, parameter);
        }

        #endregion Public Methods
    }
}