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
        private string input;

        public string PluginCategory
        {
            get { return @"Type"; }
        }

        public string Name
        {
            get { return @"SHA1"; }
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
            return Regex.IsMatch(input, @"/\b([a-f0-9]{40})\b/");
        }
    }
}