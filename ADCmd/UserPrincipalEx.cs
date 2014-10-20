using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace ADCmd
{
    [DirectoryObjectClass("user")]
    [DirectoryRdnPrefix("CN")]
    public class UserPrincipalEx : UserPrincipal
    {
        public UserPrincipalEx(PrincipalContext context) : base(context) {}

        [DirectoryProperty("company")]
        public string Company
        {
            get
            {
                if(ExtensionGet("company").Length != 1)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)ExtensionGet("company")[0];
                }
            }

            set
            {
                ExtensionSet("company", value);
            }
        }

        [DirectoryProperty("department")]
        public string Department
        {
            get
            {
                if(ExtensionGet("department").Length != 1)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)ExtensionGet("department")[0];
                }
            }

            set
            {
                ExtensionSet("department", value);
            }
        }

        [DirectoryProperty("title")]
        public string Title
        {
            get
            {
                if(ExtensionGet("title").Length != 1)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)ExtensionGet("title")[0];
                }
            }

            set
            {
                ExtensionSet("title", value);
            }
        }

        [DirectoryProperty("telephoneNumber")]
        public string PhoneNumber
        {

            get
            {
                if(ExtensionGet("telephoneNumber").Length != 1)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)ExtensionGet("telephoneNumber")[0];
                }
            }

            set
            {
                ExtensionSet("telephoneNumber", value);
            }
        }    }
}
