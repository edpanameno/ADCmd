using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;

namespace ADCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            ADDomain domain = new ADDomain();
            var options = new ProgramOptions();
            List<UserPrincipalEx> users = null;
            bool exportUsers = false;
            bool disabledUsers = false;

            var p = new OptionSet()
            {
                {"o|organizationalUnit=", "Organizational Unit that holds the users", a => options.OU = a},
                {"c|contractors=", "Get contractors from default OU", a => options.GetContractors = true },
                {"d|disabledUsers", "Get users who are disabled", a => options.GetDisabledUsers = true},
                {"e|exportUsers", "Export Users to Excel File", a => options.ExportUsers = true}
            }.Parse(args);
             
            exportUsers = options.ExportUsers;
            disabledUsers = options.GetDisabledUsers;
            
            if(String.IsNullOrEmpty(options.OU))
            {
                // Because no OU was passed, we are going to retrieve the list of users from
                // the default OU
                users = domain.GetUsersFromOU(domain.DefaultOU, disabledUsers);

                foreach(var u in users)
                {
                    Console.WriteLine(u);
                }
            }
            else
            {
                users = domain.GetUsersFromOU(options.OU, disabledUsers);
                foreach(var u in users)
                {
                    Console.WriteLine(u);
                }
            }
        }
    }

    /// <summary>
    /// This class holds the different options that can be used when running
    /// this utility from the command line.
    /// </summary>
    public class ProgramOptions
    {
        public string OU { get; set; }
        public bool GetContractors { get; set; }
        public bool GetDisabledUsers { get; set; }
        public bool ExportUsers { get; set; }
    }
}
