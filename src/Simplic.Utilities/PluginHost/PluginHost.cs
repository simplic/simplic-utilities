using Simplic.Utilities.Plugin;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Simplic.Utilities.Plugin
{
    /// <summary>
    /// Simplic plugin host. 
    /// </summary>
    public class PluginHost
    {
        private CompositionContainer _container;
        private AggregateCatalog _pluginCatalog; //An aggregate catalog that combines multiple catalogs

        /// <summary>
        /// This constructor takes parameters, creates a MEF catalog and looks for dll files in <paramref name="pluginDirectory"/> 
        /// matching the <paramref name="searchPattern"/>. 
        /// </summary>
        /// <param name="pluginDirectory">The directory to look for the plugins</param>
        /// <param name="searchPattern">A search pattern to filter out plugin dlls.</param>
        /// <param name="initLater">If this is set, the plugins Init() method wont be called imidiately.</param>
        public PluginHost(string pluginDirectory, string searchPattern, bool initLater = false)
        {
            if (string.IsNullOrWhiteSpace(pluginDirectory))
                throw new System.ArgumentNullException(nameof(pluginDirectory));

            //Adds all the parts found in the same assembly as the Program class     
            _pluginCatalog.Catalogs.Add(new DirectoryCatalog(pluginDirectory, searchPattern));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(_pluginCatalog);

            //Fill the imports of this object
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                //TODO: Log exception
                throw;                
            }

            if (!initLater)
            {
                InitAll();
            }
        }

        /// <summary>
        /// Searches for a plugin with the given name and returns it.
        /// </summary>
        /// <param name="pluginName">Plugin name to look for</param>
        /// <returns>A Plugin</returns>
        public IPlugin GetPluginByName(string pluginName)        
        {            
            foreach (var item in _container.GetExportedValues<IPlugin>())
            {
                if (item.PluginName == pluginName)                
                    return item;                
            }

            throw new System.ArgumentOutOfRangeException("Plugin does not exist.");
        }

        /// <summary>
        /// This method would iterate through all the loaded plugins and call their Init() method.
        /// </summary>
        public void InitAll()
        {
            foreach (var item in _container.GetExportedValues<IPlugin>())
            {
                item.Init();                
            }
        }        
    }
}
