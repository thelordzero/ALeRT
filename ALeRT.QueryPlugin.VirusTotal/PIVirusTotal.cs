using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.IO;
using ALeRT.PluginFramework;

namespace ALeRT.QueryPlugin
{
    public class PIVirusTotal : IQueryPlugin
    {
        public string PluginCategory
        {
            get { return @"Query"; }
        }

        public string Name
        {
            get { return @"Virus Total"; }
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
                typesAccepted.Add("File");
                //typesAccepted.Add("HASHID");
                typesAccepted.Add("IPv4");
                typesAccepted.Add("IPv6");
                typesAccepted.Add("MD5");
                //typesAccepted.Add("Name");
                //typesAccepted.Add("PhoneNumber");
                typesAccepted.Add("SHA1");
                typesAccepted.Add("SHA256");
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
                    
                }

            }
            return csv;
        }

        // Needs to accept FILE, MD5, SHA1, SHA256, or URL
        // Outputs a CSV

        private string APIKey = "REMOVED";
        private string FileReportURL = "https://www.virustotal.com/vtapi/v2/file/report";
        private string URLReportURL = "http://www.virustotal.com/vtapi/v2/url/report";
        private string URLSubmitURL = "https://www.virustotal.com/vtapi/v2/url/scan";

        WebRequest theRequest;
        HttpWebResponse theResponse;
        ArrayList theQueryData;

        public string GetFileReport(string checksum) // Gets latest report of file from VT using a hash (MD5 / SHA1 / SHA256) 
        {
            this.WebPostRequest(this.FileReportURL);
            this.Add("resource", checksum);
            return this.GetResponse();
        }

        public string GetURLReport(string url) // Gets latest report of URL from VT 
        {
            this.WebPostRequest(this.URLReportURL);
            this.Add("resource", url);
            this.Add("scan", "1"); //Automatically submits to VT if no result found 
            return this.GetResponse();
        }

        public string SubmitURL(string url) // Submits URL to VT for insertion to scanning queue 
        {
            this.WebPostRequest(this.URLSubmitURL);
            this.Add("url", url);
            return this.GetResponse();
        }

        public string SubmitFile() // Submits File to VT for insertion to scanning queue 
        {
            // File Upload code needed 
            return this.GetResponse();
        }

        private void WebPostRequest(string url)
        {
            theRequest = WebRequest.Create(url);
            theRequest.Method = "POST";
            theQueryData = new ArrayList();
            this.Add("apikey", APIKey);
        }

        private void Add(string key, string value)
        {
            theQueryData.Add(String.Format("{0}={1}", key, Uri.EscapeDataString(value)));
        }

        private string GetResponse()
        {
            // Set the encoding type 
            theRequest.ContentType = "application/x-www-form-urlencoded";

            // Build a string containing all the parameters 
            string Parameters = String.Join("&", (String[])theQueryData.ToArray(typeof(string)));
            theRequest.ContentLength = Parameters.Length;

            // We write the parameters into the request 
            StreamWriter sw = new StreamWriter(theRequest.GetRequestStream());
            sw.Write(Parameters);
            sw.Close();

            // Execute the query 
            theResponse = (HttpWebResponse)theRequest.GetResponse();
            StreamReader sr = new StreamReader(theResponse.GetResponseStream());
            return sr.ReadToEnd();
        }
    }
}
