using System;

namespace Simplic.Utilities.Plugin
{
    /// <summary>
    /// A simple Simplic.Plugin interface definition   
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// initialization method for the plugin.
        /// </summary>
        void Init();

        /// <summary>
        /// Entry point of the plugin.
        /// </summary>
        void Run();

        /// <summary>
        /// This method returns the name of the plugin.
        /// </summary>
        string PluginName();

        /// <summary>
        /// This method returns the version of the plugin.
        /// </summary>
        /// <returns></returns>
        string PluginVersion();

        /// <summary>
        /// This method returns the guid of the plugin.
        /// </summary>
        /// <returns></returns>
        Guid PluginGuid();
    }
}
