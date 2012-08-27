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
        /// Method created as IObservable as to allow QueryPlugin method to begin
        /// executing queries as results come in from DetermineTypes method.
        /// </summary>
        private IObservable<bool> DetermineTypes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method created as IObservable to provide results of the various
        /// QueryPlugins
        /// </summary>
        private IObservable<Dictionary<string, string>> QueryPlugins()
        {
            throw new NotImplementedException();
        }
    }
}
