using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using ALeRT.PluginFramework;
using System.ComponentModel.Composition;
using ALeRT.QueryPlugin.GAL;

namespace ALeRT.QueryPlugin
{
    [Export(typeof(IQueryPluginRString))]
    public class PIGAL : IQueryPluginRString
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

        public bool IsTypeAcceptable(string type)
        {
            return true;
        }

        public string Result(string input, bool sensitive)
        {
            return "";
        }
        
        public static LDAPInformation[] GetGlobalAddressListVIAMail(string email)
        {
            var currentForest = Forest.GetCurrentForest();
            var globalCatalog = currentForest.FindGlobalCatalog();

            using (var searcher = globalCatalog.GetDirectorySearcher())
            {
                using (var entry = new DirectoryEntry(searcher.SearchRoot.Path))
                {
                    searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(mail=" + email + "))";
                    //searcher.PropertiesToLoad.Add("cn");
                    searcher.PropertyNamesOnly = true;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Sort.Direction = SortDirection.Ascending;
                    searcher.Sort.PropertyName = "displayName";
                    return searcher.FindAll().Cast<SearchResult>().Select(result => new LDAPInformation(result.GetDirectoryEntry())).ToArray();
                }
            }
        }

        public static LDAPInformation[] GetGlobalAddressListVIAHASH(string hash)
        {
            var currentForest = Forest.GetCurrentForest();
            var globalCatalog = currentForest.FindGlobalCatalog();

            using (var searcher = globalCatalog.GetDirectorySearcher())
            {
                using (var entry = new DirectoryEntry(searcher.SearchRoot.Path))
                {
                    searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(sAMAccountName=" + hash + "))";
                    //searcher.PropertiesToLoad.Add("cn");
                    searcher.PropertyNamesOnly = true;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Sort.Direction = SortDirection.Ascending;
                    searcher.Sort.PropertyName = "displayName";
                    return searcher.FindAll().Cast<SearchResult>().Select(result => new LDAPInformation(result.GetDirectoryEntry())).ToArray();
                }
            }
        }

        public static LDAPInformation[] GetGlobalAddressListVIAName(string nameQuery)
        {
            var currentForest = Forest.GetCurrentForest();
            var globalCatalog = currentForest.FindGlobalCatalog();

            using (var searcher = globalCatalog.GetDirectorySearcher())
            {
                using (var entry = new DirectoryEntry(searcher.SearchRoot.Path))
                {
                    searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(anr=" + nameQuery + "))";
                    //searcher.PropertiesToLoad.Add("cn");
                    searcher.PropertyNamesOnly = true;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Sort.Direction = SortDirection.Ascending;
                    searcher.Sort.PropertyName = "displayName";
                    return searcher.FindAll().Cast<SearchResult>().Select(result => new LDAPInformation(result.GetDirectoryEntry())).ToArray();
                }
            }
        }
    }
}
