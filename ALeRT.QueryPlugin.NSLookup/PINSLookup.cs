using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel.Composition;
using ALeRT.PluginFramework;
using System.IO;
using System.Management;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ALeRT.QueryPlugin
{
    [Export(typeof(IQueryPlugin))]
    public class PINSLookup : IQueryPlugin
    {
        public string PluginCategory
        {
            get { return @"Query"; }
        }

        public string Name
        {
            get { return @"NSLookup"; }
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
                if (type == "URL")
                {
                    input = new Uri(input).Host;
                }

                Process scanProcess = new Process();

                scanProcess.StartInfo.RedirectStandardError = true;
                scanProcess.StartInfo.RedirectStandardOutput = true;
                scanProcess.StartInfo.UseShellExecute = false;
                scanProcess.StartInfo.FileName = "cmd.exe";
                scanProcess.StartInfo.Arguments = "/c nslookup " + input;
                //scanProcess.StartInfo.Arguments = "/c nslookup " + input + " 8.8.8.8"; // This utilizes Google's DNS
                scanProcess.StartInfo.CreateNoWindow = true;
                scanProcess.Start();

                StreamReader sOut = scanProcess.StandardOutput;
                StringBuilder result = new StringBuilder();
                string temp;

                while ((temp = sOut.ReadLine()) != null)
                {
                    result.AppendLine(temp);
                }

                sOut = scanProcess.StandardError;

                while ((temp = sOut.ReadLine()) != null)
                {
                    result.AppendLine(temp);
                }

                sOut.Close();
                scanProcess.Close();

                char[] delimiterChars = { ',', '\t', '\r', '\n' };
                string[] words = result.ToString().Split(delimiterChars);


                foreach (string s in words)
                {
                    if (s != "")
                    {
                        csv += "\"" + "Output" + "\"," + "\"" + s.Trim().ToString() + "\"\n";
                    }
                }
            }
            return csv;
        }
    }
}
