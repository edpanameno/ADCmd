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
    /// <summary>
    /// Specifies what AD Property will be used to search
    /// objects in active directory.
    /// </summary>
    public enum SearchProperty 
    {
        FirstName = 1,
        LastName,
        Email,
        Department,
        Company
    }

    public class ADDomain
    {
        public string ServerName { get; set; }
        public string Container { get; set; }
        public string LDAPPath { get; set; }
        public string ServiceUser { get; set; }
        public string ServicePassword { get; set; }
        public string DefaultOU { get; set; }

        public ADDomain()
        {
            LDAPPath =  ConfigurationManager.AppSettings["ldap_path"];
            ServerName = ConfigurationManager.AppSettings["server_name"];
            Container = ConfigurationManager.AppSettings["container"];
            DefaultOU = ConfigurationManager.AppSettings["default-ou"];
            ServiceUser = ConfigurationManager.AppSettings["service_user"];
            ServicePassword = ConfigurationManager.AppSettings["service_password"];
        }

        public List<UserPrincipalEx> GetAllContractors()
        {
            List<UserPrincipalEx> activeUsers = new List<UserPrincipalEx>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain, 
                                                            ServerName, 
                                                            DefaultOU, 
                                                            ContextOptions.Negotiate, 
                                                            ServiceUser, 
                                                            ServicePassword);

            // We are only interested in searching for active directory 
            // accounts that are enabled.
            UserPrincipalEx userFilter = new UserPrincipalEx(context)
            {
                Enabled = true 
            };

            using(PrincipalSearcher searcher = new PrincipalSearcher(userFilter))
            {
                ((DirectorySearcher)searcher.GetUnderlyingSearcher()).PageSize = 1000;
                var searchResults = searcher.FindAll().ToList();

                foreach(Principal user in searchResults) 
                {
                    // This will allow us to get to the custom attributes that we 
                    // have defined in our custom UserPrincipal object
                    UserPrincipalEx usr = user as UserPrincipalEx;
                    activeUsers.Add(usr);
                }
            }

            return activeUsers.OrderBy(u => u.Surname).ToList();
        }
        
        public List<UserPrincipalEx> GetDisabledUsers()
        {
            List<UserPrincipalEx> activeUsers = new List<UserPrincipalEx>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain, 
                                                            ServerName, 
                                                            Container, 
                                                            ContextOptions.Negotiate, 
                                                            ServiceUser, 
                                                            ServicePassword);

            // We are only interested in searching for active directory 
            // accounts that are disabled.
            UserPrincipalEx userFilter = new UserPrincipalEx(context)
            {
                Enabled = false 
            };

            using(PrincipalSearcher searcher = new PrincipalSearcher(userFilter))
            {
                ((DirectorySearcher)searcher.GetUnderlyingSearcher()).PageSize = 1000;
                var searchResults = searcher.FindAll().ToList();

                foreach(Principal user in searchResults) 
                {
                    // This will allow us to get to the custom attributes that we 
                    // have defined in our custom UserPrincipal object
                    UserPrincipalEx usr = user as UserPrincipalEx;
                    activeUsers.Add(usr);
                }
            }

            return activeUsers.OrderBy(u => u.Surname).ToList();
        }

        /// <summary>
        /// Gets all of the users from the specified OU. The ou va
        /// </summary>
        /// <param name="ou"></param>
        /// <returns></returns>
        public List<UserPrincipalEx> GetUsersFromOU(string ouDN)
        {
            List<UserPrincipalEx> users = new List<UserPrincipalEx>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain, 
                                                            ServerName, 
                                                            ouDN, 
                                                            ContextOptions.Negotiate, 
                                                            ServiceUser, 
                                                            ServicePassword);

            UserPrincipalEx userFilter = new UserPrincipalEx(context);

            using(PrincipalSearcher searcher = new PrincipalSearcher(userFilter))
            {
                ((DirectorySearcher)searcher.GetUnderlyingSearcher()).PageSize = 1000;
                var searchResults = searcher.FindAll().ToList();

                foreach(Principal user in searchResults) 
                {
                    // This will allow us to get to the custom attributes that we 
                    // have defined in our custom UserPrincipal object
                    UserPrincipalEx usr = user as UserPrincipalEx;
                    users.Add(usr);
                }
            }

            return users.OrderBy(u => u.Surname).ToList();
        }

        /// <summary>
        /// Checks to see if the OU DN is a valid one.
        /// </summary>
        /// <param name="ou"></param>
        /// <returns>True is OU exists, false otherwise.</returns>
        public bool IsValidOU(string ouDistinguishedName)
        {
            bool result = true;
            PrincipalContext context = null;

            try
            {
                context = new PrincipalContext(ContextType.Domain, 
                                               ServerName, 
                                               ouDistinguishedName, 
                                               ContextOptions.Negotiate, 
                                               ServiceUser, 
                                               ServicePassword);
            }
            catch(Exception e)
            {
                result = false;
            }
            finally
            {
                context.Dispose();
            }

            return result;
        }
    }
}
