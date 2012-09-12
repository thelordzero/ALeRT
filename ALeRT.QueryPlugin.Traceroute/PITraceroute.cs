using ALeRT.PluginFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace ALeRT.QueryPlugin
{
    public class PITraceroute
    {
        [Export(typeof(IQueryPlugin))]
        public class PIPing : IQueryPlugin
        {
            public string PluginCategory
            {
                get { return @"Type"; }
            }

            public string Name
            {
                get { return @"Traceroute"; }
            }

            public string Version
            {
                get { return @"1.0.0"; }
            }

            public string Author
            {
                get { return @"John"; }
            }

            public List<string> TypesAccepted
            {
                get
                {
                    List<string> typesAccepted = new List<string>();
                    //typesAccepted.Add("EMail");
                    //typesAccepted.Add("File");
                    //typesAccepted.Add("HASHID");
                    typesAccepted.Add("IPv4");
                    typesAccepted.Add("IPv6");
                    //typesAccepted.Add("MD5");
                    //typesAccepted.Add("Name");
                    //typesAccepted.Add("PhoneNumber");
                    //typesAccepted.Add("SHA1");
                    //typesAccepted.Add("SHA256");
                    //typesAccepted.Add("URL");
                    //typesAccepted.Add("ZipCode");
                    return typesAccepted;
                }
            }

            public string Result(string input, string type, bool sensitive)
            {
                return null;
            }
        }
    }
}
