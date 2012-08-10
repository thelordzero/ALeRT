using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALeRT.PluginFramework
{
    interface IQueryPlugin
    {
        string Name { get; set; }
        bool Result { get; set; }
    }
}
