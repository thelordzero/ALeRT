using System;
using System.Text;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using ALeRT.PluginFramework;

namespace ALeRT.TypePlugin
{
    [Export(typeof(ITypePlugin))]
    public class PIURL : ITypePlugin
    {
        public string PluginCategory
        {
            get { return @"Type"; }
        }

        public string Name
        {
            get { return @"URL"; }
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
            return Regex.IsMatch(input, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }
    }
}