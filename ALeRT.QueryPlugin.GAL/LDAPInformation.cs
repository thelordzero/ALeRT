using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace ALeRT.QueryPlugin.GAL
{
    public class LDAPInformation
    {
        public LDAPInformation(DirectoryEntry entry)
        {
            //Section: HASH
            this.sAMAccountName = (string)entry.Properties["sAMAccountName"].Value;

            //Section: Email
            this.Mail = (string)entry.Properties["mail"].Value;

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
    }
}
