namespace Simplic.SAAP
{
    /// <summary>
    /// provider main interface, must be implemented in all provider implementations
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Initialize the Provider
        /// </summary>
        /// <param name="parameter">Parameters</param>
        /// <returns>True, if the provider was successfuly initialize</returns>
        bool Initialize(params object[] parameter);

        /// <summary>
        /// Will be called when the provider is activated
        /// </summary>
        void Activate();

        /// <summary>
        /// Will be called when the provider is deactivated
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Will be called when the provider was removed
        /// </summary>
        void Remove();

        /// <summary>
        /// Shutdown the Provider
        /// </summary>
        /// <returns>Returns true, when the provider was shutdown successfuly</returns>
        bool Shutdown();

        /// <summary>
        /// Defines, wether a provider is installed
        /// </summary>
        bool IsInstalled
        {
            get;
        }

        /// <summary>
        /// Name of the Provider
        /// </summary>
        string ProviderName
        {
            get;
        }
    }
}