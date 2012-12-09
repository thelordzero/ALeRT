using ALeRT.PluginFramework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
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
                    searcher.ReferralChasing = ReferralChasingOption.All;

                    using (var entry = new DirectoryEntry(searcher.SearchRoot.Path))
                    {
                        if (type == "EMail")
                        {
                            //searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(mail=" + input + "))";
                            searcher.Filter = "(&(mailnickname=*)(objectCategory=user)(proxyAddresses=smtp:" + input + "))";
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

                        LDAPInformation[] temp = (searcher.FindAll().Cast<SearchResult>().Select(result => new LDAPInformation(result.GetDirectoryEntry())).ToArray());

                        string csv = "\"" + "Display Name" + "\"," + "\"" + "EMail" + "\"," + "\"" + "HASH" + "\"," + "\"" + "Phone" + "\"," + "\"" + "Company" + "\"," + "\"" + "Department" + "\"," + "\"" + "Account Flags" + "\"," + "\"" + "Last Logon" + "\"," + "\"" + "Proxy Addresses" + "\"," + "\"" + "Member Of" + "\"\n";

                        foreach (LDAPInformation t in temp)
                        {
                            csv += "\"" + t.DisplayName + "\"," + "\"" + t.Mail + "\"," + "\"" + t.sAMAccountName + "\"," + "\"" + t.TelephoneNumber + "\"," + "\"" + t.Company + "\"," + "\"" + t.Department + "\"," + "\"" + t.AccountFlags + "\"," + "\"" + t.LastLogon + "\"," + "\"" + t.ProxyAddresses + "\"," + "\"" + t.MemberOf + "\"\n";
                        }

                        return csv;
                    }
                }
            }
            catch (ActiveDirectoryOperationException e)
            {
                return e.Message;
            }
        }
    }

    class LDAPInformation
    {
        internal LDAPInformation(DirectoryEntry entry)
        {
            //Section: HASH
            this.sAMAccountName = (string)entry.Properties["sAMAccountName"].Value;

            //Section: Email
            this.Mail = (string)entry.Properties["mail"].Value;
            foreach (string proxyAddr in entry.Properties["proxyAddresses"])
            {
                //Make it 'case-insensative'
                if (proxyAddr.ToLower().StartsWith("smtp:"))
                    //Get the email string from AD
                    this.ProxyAddresses += proxyAddr.Substring(5) + "|";
            }
            if (this.ProxyAddresses != null)
            {
                this.ProxyAddresses = this.ProxyAddresses.Remove(this.ProxyAddresses.Length - 1);
            }

            //Section: Organziation
            this.Description = (string)entry.Properties["description"].Value;
            this.Company = (string)entry.Properties["company"].Value;
            this.Title = (string)entry.Properties["title"].Value;
            this.Department = (string)entry.Properties["department"].Value;

            //Section: Name
            this.DisplayName = (string)entry.Properties["displayName"].Value;
            this.FirstName = (string)entry.Properties["firstName"].Value;
            this.MiddleName = (string)entry.Properties["middleName"].Value;
            this.LastName = (string)entry.Properties["lastName"].Value;

            //Section: Address
            this.StreetAddress = (string)entry.Properties["streetAddress"].Value;
            this.City = (string)entry.Properties["city"].Value;
            this.State = (string)entry.Properties["state"].Value;
            this.PostalCode = (string)entry.Properties["postalCode"].Value;
            this.TelephoneNumber = (string)entry.Properties["telephoneNumber"].Value;

            //Section: Administrative
            if (entry.Properties["userAccountControl"].Value != null)
            {
                UserAccountControlFlags userAccountControl = (UserAccountControlFlags)entry.Properties["userAccountControl"].Value;
                this.AccountFlags = userAccountControl.ToString();
            }
            this.LastLogon = (string)entry.Properties["lastLogon"].Value;
            
            foreach (string memOf in entry.Properties["memberOf"])
            {
                //this.MemberOf += memOf.ToString() + "|";
                string[] t0 = memOf.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] t1 = t0[0].Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                this.MemberOf += t1[1].ToString() + "|";
            }
            if (this.MemberOf != null)
            {
                this.MemberOf = this.MemberOf.Remove(this.MemberOf.Length - 1);
            }
        }

        public string DisplayName
        {
            get;
            private set;
        }

        public string Mail
        {
            get;
            private set;
        }

        public string ProxyAddresses
        {
            get;
            private set;
        }

        public string sAMAccountName
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public string Company
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public string Department
        {
            get;
            private set;
        }

        public string FirstName
        {
            get;
            private set;
        }

        public string MiddleName
        {
            get;
            private set;
        }

        public string LastName
        {
            get;
            private set;
        }

        public string StreetAddress
        {
            get;
            private set;
        }

        public string City
        {
            get;
            private set;
        }

        public string State
        {
            get;
            private set;
        }

        public string PostalCode
        {
            get;
            private set;
        }

        public string TelephoneNumber
        {
            get;
            private set;
        }

        public string AccountFlags
        {
            get;
            private set;
        }

        public string LastLogon
        {
            get;
            private set;
        }

        public string MemberOf
        {
            get;
            private set;
        }

        [Flags]
        public enum UserAccountControlFlags
        {
            Script = 0x1,
            AccountDisabled = 0x2,
            HomeDirectoryRequired = 0x8,
            AccountLockedOut = 0x10,
            PasswordNotRequired = 0x20,
            PasswordCannotChange = 0x40,
            EncryptedTextPasswordAllowed = 0x80,
            TempDuplicateAccount = 0x100,
            NormalAccount = 0x200,
            InterDomainTrustAccount = 0x800,
            WorkstationTrustAccount = 0x1000,
            ServerTrustAccount = 0x2000,
            PasswordDoesNotExpire = 0x10000,
            MnsLogonAccount = 0x20000,
            SmartCardRequired = 0x40000,
            TrustedForDelegation = 0x80000,
            AccountNotDelegated = 0x100000,
            UseDesKeyOnly = 0x200000,
            DontRequirePreauth = 0x400000,
            PasswordExpired = 0x800000,
            TrustedToAuthenticateForDelegation = 0x1000000,
            NoAuthDataRequired = 0x2000000
        }
    }
}