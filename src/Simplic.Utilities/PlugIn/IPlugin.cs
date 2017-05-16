using System;

namespace Simplic.Utilities.Plugin
{
    /// <summary>
    /// A simple Simplic.Plugin interface   
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
        /// The name of the plugin.
        /// </summary>
        string PluginName { get; }

        /// <summary>
        /// The plugin version
        /// </summary>
        string PluginVersion { get; }

        /// <summary>
        /// The guid of the plugin.
        /// </summary>
        Guid PluginId { get; }       
    }
}
