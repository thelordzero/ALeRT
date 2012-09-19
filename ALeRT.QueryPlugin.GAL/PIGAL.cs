using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using ALeRT.PluginFramework;
using System.ComponentModel.Composition;
using Newtonsoft.Json;
using System.IO;

namespace ALeRT.QueryPlugin
{
    [Export(typeof(IQueryPlugin))]
    public class PIGAL : IQueryPlugin
    {
        public string PluginCategory
        {
            get { return @"Query"; }
        }

        public string Name
        {
            get { return @"GAL"; }
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
                typesAccepted.Add("EMail");
                //typesAccepted.Add("File");
                typesAccepted.Add("HASHID");
                //typesAccepted.Add("IPv4");
                //typesAccepted.Add("IPv6");
                //typesAccepted.Add("MD5");
                typesAccepted.Add("Name");
                typesAccepted.Add("PhoneNumber");
                //typesAccepted.Add("SHA1");
                //typesAccepted.Add("SHA256");
                //typesAccepted.Add("URL");
                typesAccepted.Add("ZipCode");
                return typesAccepted;
            }
        }

        public string Result(string input, string type, bool sensitive)
        {
            try
            {
                var currentForest = Forest.GetCurrentForest();
                var globalCatalog = currentForest.FindGlobalCatalog();

                using (var searcher = globalCatalog.GetDirectorySearcher())
                {
                    using (var entry = new DirectoryEntry(searcher.SearchRoot.Path))
                    {
                        if (type == "EMail")
                        {
                            searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(mail=" + input + "))";
                        }

                        if (type == "HASHID")
                        {
                            searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(sAMAccountName=" + input + "))";
                        }

                        if (type == "Name")
                        {
                            searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(anr=" + input + "))";
                        }

                        if (type == "PhoneNumber")
                        {
                            searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(telephoneNumber=" + input + "))";
                        }

                        if (type == "ZipCode")
                        {
                            searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(postalCode=" + input + "))";
                        }

                        searcher.PropertyNamesOnly = true;
                        searcher.SearchScope = SearchScope.Subtree;
                        searcher.Sort.Direction = SortDirection.Ascending;
                        searcher.Sort.PropertyName = "displayName";

                        return JsonConvert.SerializeObject(searcher.FindAll().Cast<SearchResult>().Select(result => result.GetDirectoryEntry()));
                    }
                }
            }
            catch (ActiveDirectoryOperationException e)
            {
                return null;
            }
        }
    }
}
