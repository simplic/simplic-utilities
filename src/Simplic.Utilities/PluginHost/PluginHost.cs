using Simplic.Utilities.Plugin;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Simplic.Utilities.Plugin
{
    public class PluginHost
    {
        private CompositionContainer _container;
        private AggregateCatalog _pluginCatalog; //An aggregate catalog that combines multiple catalogs

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

        public IPlugin GetPluginByName(string pluginName)        
        {            
            foreach (var item in _container.GetExportedValues<IPlugin>())
            {
                if (item.PluginName() == pluginName)                
                    return item;                
            }

            throw new System.ArgumentOutOfRangeException("Plugin does not exist.");
        }

        public void InitAll()
        {
            foreach (var item in _container.GetExportedValues<IPlugin>())
            {
                item.Init();                
            }
        }        
    }
}
