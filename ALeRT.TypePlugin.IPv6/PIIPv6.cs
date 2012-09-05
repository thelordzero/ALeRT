using System;
using System.Text;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using ALeRT.PluginFramework;

namespace ALeRT.TypePlugin
{
    [Export(typeof(ITypePlugin))]
    public class PIIPv6 : ITypePlugin
    {
        public string PluginCategory
        {
            get { return @"Type"; }
        }

        public string Name
        {
            get { return @"IPv6"; }
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
            return Regex.IsMatch(input, @"^([0-9a-fA-F]{4}|0)(\:([0-9a-fA-F]{4}|0)){7}$");
        }
    }
}
        // ^([0-9a-fA-F]{4}|0)(\:([0-9a-fA-F]{4}|0)){7}$

        // Or the following four
        // Const strIPv6Pattern as string = "\A(?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}\z"
        // Const strIPv6Pattern_HEXCompressed as string = "\A((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)::((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)\z"
        // Const StrIPv6Pattern_6Hex4Dec as string = "\A((?:[0-9A-Fa-f]{1,4}:){6,6})(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}\z"
        // Const StrIPv6Pattern_Hex4DecCompressed as string = "\A((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?) ::((?:[0-9A-Fa-f]{1,4}:)*)(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}\z"