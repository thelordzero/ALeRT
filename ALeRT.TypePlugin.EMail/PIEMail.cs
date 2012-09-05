using System;
using System.Text;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using ALeRT.PluginFramework;

namespace ALeRT.TypePlugin
{
    [Export(typeof(ITypePlugin))]
    public class PIEMail : ITypePlugin
    {
        public string PluginCategory
        {
            get { return @"Type"; }
        }

        public string Name
        {
            get { return @"EMail"; }
        }

        public string Version
        {
            get { return @"1.0.0"; }
        }

        public string Author
        {
            get { return @"John"; }
        }

        public bool Result(string input)
        {
            return Regex.IsMatch(input, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }
    }
}
            //Regex reg = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            //Other option: ^[A-Za-z0-9_\-\.]+@(([A-Za-z0-9\-])+\.)+([A-Za-z\-])+$