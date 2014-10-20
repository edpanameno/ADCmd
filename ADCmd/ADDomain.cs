using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADCmd
{
    public class ADDomain
    {
        public string ServerName { get; set; }
        public string Container { get; set; }
        public string LDAPPath { get; set; }
        public string ServiceUser { get; set; }
        public string ServicePassword { get; set; }
        public string ContractorsOU { get; set; }


        public ADDomain()
        {
            LDAPPath =  ConfigurationManager.AppSettings["ldap_path"];
            ServerName = ConfigurationManager.AppSettings["server_name"];
            Container = ConfigurationManager.AppSettings["container"];
            ContractorsOU = ConfigurationManager.AppSettings["contractors-ou"];
            ServiceUser = ConfigurationManager.AppSettings["service_user"];
            ServicePassword = ConfigurationManager.AppSettings["service_password"];
        }

        public List<UserPrincipalEx> GetAllContractors()
        {
            List<UserPrincipalEx> activeUsers = new List<UserPrincipalEx>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain, 
                                                            ServerName, 
                                                            ContractorsOU, 
                                                            ContextOptions.Negotiate, 
                                                            ServiceUser, 
                                                            ServicePassword);

            UserPrincipalEx userPrincipal = new UserPrincipalEx(context)
            {
                // We are only interested in searching for active directory 
                // accounts that are enabled.
                Enabled = true
            };

            PrincipalSearcher searcher = new PrincipalSearcher();
            searcher.QueryFilter = userPrincipal;
            ((DirectorySearcher)searcher.GetUnderlyingSearcher()).PageSize = 1000;
            var searchResults = searcher.FindAll().ToList();

            foreach(UserPrincipalEx user in searchResults) 
            {
                activeUsers.Add(user);
            }

            return activeUsers;
        }
    }
}
