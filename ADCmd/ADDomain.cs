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

        /// <summary>
        /// Gets a list of users from the Organizational Unit that has been specified
        /// as the default OU in the app.config file.
        /// </summary>
        /// <returns></returns>
        public List<UserPrincipalEx> GetUsersFromDefaultOU()
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
       
        /// <summary>
        /// Gets a list of users who are disabled
        /// </summary>
        /// <returns></returns>
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
        /// <param name="ouDN">The Distinguished DN of the OU to get users from</param>
        /// <param name="getDisabledUsers">Boolean value used to check if you want to get disabled users or not.</param>
        /// <returns></returns>
        public List<UserPrincipalEx> GetUsersFromOU(string ouDN, bool getDisabledUsers)
        {
            List<UserPrincipalEx> users = new List<UserPrincipalEx>();

            // The PrincipalContext object is used to establish a connection to the
            // target directory and specify the credentials for performing
            // operations against the directory. In the example below, we are 
            // connecting to a Active Directory Domain.
            // The ServerName parameter that you see below can be either a server name
            // (i.e. domain controller) or domain name to connect to.
            PrincipalContext context = new PrincipalContext(ContextType.Domain, 
                                                            ServerName, 
                                                            ouDN, 
                                                            ContextOptions.Negotiate, 
                                                            ServiceUser, 
                                                            ServicePassword);


            UserPrincipalEx userFilter = new UserPrincipalEx(context);

            if(!getDisabledUsers)
            {
                userFilter.Enabled = false;
            }

            using(PrincipalSearcher searcher = new PrincipalSearcher())
            {
                searcher.QueryFilter = userFilter;
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
