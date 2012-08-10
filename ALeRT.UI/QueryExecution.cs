using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace ALeRT
{
    class QueryExecution
    {
        QueryExecution(string query)
        {
            // Take query from UI and execute DetermineTypes method. Should return a list
            // with what plugins return TRUE or FALSE from a boolean type. All values in
            // the list that are TRUE are passed onto the QueryPlugins for evauluation. Unsure
            // as of what type it should return to post to the UI. Assuming it could be a
            // dynamic object that gets itterated through. Additionally, all plugins should 
            // be running asynchronously, possibly by utilizing System.Reactive outside of the 
            // actual plugins.
            DetermineTypes();
            this.QueryPlugins();
        }

        /// <summary>
        /// Method created as ObservableCollection as to allow QueryPlugin method to begin
        /// executing queries as results come in from DetermineTypes method.
        /// </summary>
        //private ObservableCollection<string> DetermineTypes
        private void DetermineTypes()
        {
            throw new NotImplementedException();
            //Collection<string> results = Collection<string>();
            // var results;
            // Need to 
            // return results;
        }

        /// <summary>
        /// Method created as ObservableCollection to provide results of the various
        /// QueryPlugins
        /// </summary>
        //private ObservableCollection<string> QueryPlugins
        private void QueryPlugins()
        {
            throw new NotImplementedException();
        }
    }
}
