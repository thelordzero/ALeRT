using System;
using System.Text;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using ALeRT.PluginFramework;

namespace ALeRT.TypePlugin
{
    [Export(typeof(ITypePlugin))]
    public class PIMD5 : ITypePlugin
    {
        public string PluginCategory
        {
            get { return @"Type"; }
        }

        public string Name
        {
            get { return @"MD5"; }
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
            return Regex.IsMatch(input, @"#^[0-9a-f]{32}$#i");
        }
    }
}