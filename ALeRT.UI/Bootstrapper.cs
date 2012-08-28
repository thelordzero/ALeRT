using System.Collections.Generic;
using System.ComponentModel.Composition;
using ALeRT.PluginFramework;

namespace ALeRT
{
    public class Bootstrapper
    {
        /// <summary>
        /// Holds a list of all the valid Type plugins for this application
        /// </summary>
        [ImportMany]
        public IEnumerable<ITypePlugin> TPlugins { get; set; }
        public IEnumerable<ITypePlugin> _tPlugin;
    }
}