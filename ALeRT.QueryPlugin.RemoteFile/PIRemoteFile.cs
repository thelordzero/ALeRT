using ALeRT.PluginFramework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Newtonsoft.Json;

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
                //typesAccepted.Add("URL");
                //typesAccepted.Add("ZipCode");
                return typesAccepted;
            }
        }

        public string Result(string input, string type, bool sensitive)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                if (sensitive == true)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName(this.Name);
                    writer.WriteValue("FORBIDDEN");
                    writer.WriteEndObject();
                }
                else
                {
                    try
                    {
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(input, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\\\" + Path.GetFileName(new Uri(input).LocalPath));

                        writer.WriteStartObject();
                        writer.WritePropertyName("Download");
                        writer.WriteValue("Successful");
                        writer.WriteEndObject();
                    }
                    catch (WebException e)
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("Error");
                        writer.WriteValue(e.Message);
                        writer.WriteEndObject();
                    }
                }
                return sw.ToString();
            }
        }
    }
}