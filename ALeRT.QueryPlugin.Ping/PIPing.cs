using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.ComponentModel.Composition;
using ALeRT.PluginFramework;
using System.IO;

namespace ALeRT.QueryPlugin
{
    [Export(typeof(IQueryPlugin))]
    public class PIPing : IQueryPlugin
    {
        public string PluginCategory
        {
            get { return @"Query"; }
        }

        public string Name
        {
            get { return @"Ping"; }
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
            get { 
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
                typesAccepted.Add("URL");
                //typesAccepted.Add("ZipCode");
                return typesAccepted;
            }
        }

        public string Result(string input, string type, bool sensitive)
        {
            string csv = "\"Roundtrip Time\"," + "\"Status\"\n";

            if (sensitive == true)
            {
                csv += "\"" + "" + "\"," + "\"" + "FORBIDDEN" + "\"\n";
            }
            else
            {
                if (type == "URL")
                {
                    input = new Uri(input).Host;
                }
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(input);

                csv += "\"" + pingReply.RoundtripTime.ToString() + "\"," + "\"" + pingReply.Status.ToString() + "\"\n";
            }
            return csv;
        }
    }
}
