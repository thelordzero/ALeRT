using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.IO;

namespace ALeRT
{
    public class QueryExecution
    {
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
            var bootStrapper = new Bootstrapper();

            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in same directory where the application is running!
            var currentPath = Directory.GetCurrentDirectory();
            catalog.Catalogs.Add(new DirectoryCatalog(currentPath));

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                container.ComposeParts(bootStrapper);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

            //Prints all the languages that were found into the application directory
            var i = 0;
            foreach (var language in bootStrapper.TPlugins)
            {
                //Console.WriteLine("[{0}] {1} by {2}.\n\t{3}\n", language.Version, language.Name, language.Author, language.Result);
                i++;
                
            }

            //throw new NotImplementedException();
        }

        private void QueryPlugins()
        {
            throw new NotImplementedException();
        }
    }
}
