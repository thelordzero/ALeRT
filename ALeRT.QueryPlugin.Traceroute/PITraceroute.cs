using ALeRT.PluginFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace ALeRT.QueryPlugin
{
    [Export(typeof(IQueryPlugin))]
    public class PITraceroute : IQueryPlugin
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
                    int iHopcount = 30;
                    int iTimeout = 500;

                    System.Collections.ArrayList arlPingReply = new System.Collections.ArrayList();
                    Ping myPing = new Ping();
                    PingReply prResult;

                    for (int iC1 = 1; iC1 < iHopcount; iC1++)
                    {
                        prResult = myPing.Send(input, iTimeout, new byte[10], new PingOptions(iC1, false));
                        if (prResult.Status == IPStatus.Success)
                        {
                            iC1 = iHopcount;
                        }
                        arlPingReply.Add(prResult);
                    }

                    PingReply[] prReturnValue = new PingReply[arlPingReply.Count];

                    for (int iC1 = 0; iC1 < arlPingReply.Count; iC1++)
                    {
                        prReturnValue[iC1] = (PingReply)arlPingReply[iC1];

                        writer.WriteStartObject();
                        writer.WritePropertyName("Address");
                        if (prReturnValue[iC1].Address.ToString() != null)
                        {
                            writer.WriteValue(prReturnValue[iC1].Address.ToString());
                        }
                        writer.WritePropertyName("RoundtripTime");
                        writer.WriteValue(prReturnValue[iC1].RoundtripTime.ToString());
                        writer.WritePropertyName("Status");
                        writer.WriteValue(prReturnValue[iC1].Status.ToString());
                        writer.WriteEndObject();
                    }
                }
                return sw.ToString();
            }
        }
    }
}