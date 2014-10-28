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

            var p = new OptionSet()
            {
                {"o|organizationalUnit=", "Organizational Unit that holds the users", a => options.OU = a},
                {"dou|defaultOU=", "Get Users from the Organization Unit", a => options.GetDefaultOU = true},
                {"c|contractors=", "Get contractors from default OU", a => options.GetContractors = true },
                {"d|disabledUsers", "Get users who are disabled", a => options.GetDisabledUsers = true},
                {"e|exportUsers", "Export Users to Excel File", a => options.ExportUsers = true}
            }.Parse(args);
            
            if(!String.IsNullOrEmpty(options.OU))
            {
                // This is a bit of a hack to get the proper format for the 
                // OU DN, this will get fixed (see issue #5).
                string ou = options.OU.Substring(1, options.OU.Length - 1);
                Console.WriteLine("OU: " + ou);
                bool exportUsers = options.ExportUsers;
                List<UserPrincipalEx> users = domain.GetUsersFromOU(ou, exportUsers);

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
        public bool GetDefaultOU { get; set; }
        public bool ExportUsers { get; set; }
    }
}
