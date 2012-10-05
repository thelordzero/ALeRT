using ALeRT.PluginFramework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ALeRT.QueryPlugin
{
    [Export(typeof(IQueryPlugin))]
    public class PIRemoteFile : IQueryPlugin
    {
        public string PluginCategory
        {
            get { return @"Query"; }
        }

        public string Name
        {
            get { return @"RemoteFile"; }
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
                //typesAccepted.Add("IPv4");
                //typesAccepted.Add("IPv6");
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
            string csv = "\"Status\"," + "\"Message\"\n";
            
            if (sensitive == true)
            {
                csv += "\"" + "FORBIDDEN" + "\"," + "\"" + "" + "\"\n";
            }
            else
            {
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(input, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\\\" + Path.GetFileName(new Uri(input).LocalPath));

                    csv += "\"" + "Successful" + "\"," + "\"" + "" + "\"\n";
                }
                catch (WebException e)
                {
                    csv += "\"" + "Error" + "\"," + "\"" + e.Message + "\"\n";
                }
            }
            return csv;

        }
    }
}