using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALeRT.TypePlugin
{
    public class PIIPv6
    {
        // ^([0-9a-fA-F]{4}|0)(\:([0-9a-fA-F]{4}|0)){7}$

        // Or the following four
        // Const strIPv6Pattern as string = "\A(?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}\z"
        // Const strIPv6Pattern_HEXCompressed as string = "\A((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)::((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)\z"
        // Const StrIPv6Pattern_6Hex4Dec as string = "\A((?:[0-9A-Fa-f]{1,4}:){6,6})(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}\z"
        // Const StrIPv6Pattern_Hex4DecCompressed as string = "\A((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?) ::((?:[0-9A-Fa-f]{1,4}:)*)(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}\z"
    }
}
