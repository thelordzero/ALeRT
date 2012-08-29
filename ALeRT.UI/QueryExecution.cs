using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.IO;
using ALeRT.PluginFramework;

namespace ALeRT
{
    public class QueryExecution
    {
        [ImportMany]
        public IEnumerable<ITypePlugin> TPlugins { get; set; }

        public QueryExecution()
        {
            // Take query from UI and execute DetermineTypes method. Should return a list
            // with what plugins return TRUE or FALSE from a boolean type. All values in
            // the list that are TRUE are passed onto the QueryPlugins for evauluation. Unsure
            // as of what type it should return to post to the UI. Assuming it could be a
            // dynamic object that gets itterated through. Additionally, all plugins should 
            // be running asynchronously, possibly by utilizing System.Reactive outside of the 
            // actual plugins.
            DetermineTypes();
            //QueryPlugins();
        }

        /// <summary>
        /// Method to load all Type Plugins.
        /// </summary>
        private void DetermineTypes()
        {
            var catalog = new AggregateCatalog();

            var currentPath = Directory.GetCurrentDirectory();
            catalog.Catalogs.Add(new DirectoryCatalog(currentPath));

            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (var tPlugins in this.TPlugins)
            {
                System.Windows.MessageBox.Show("Category: " + tPlugins.PluginCategory + "\nName: " + tPlugins.Name +"\nThe result is: " + tPlugins.Result("teststuff"));
            }
        }

        private void QueryPlugins()
        {
            throw new NotImplementedException();
        }
    }
}
