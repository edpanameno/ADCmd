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
            var options = new ProgramOptions();

            string ouContainer = string.Empty;
            var p = new OptionSet()
            {
                {"ou|organizationalUnit=", "Organizational Unit that holds the users", a => options.OU = a},
                {"c|contractors=", "Get contractors from default OU", a => options.GetContractors = true }
            }.Parse(args);

            if(options.OU != null)
            {
                Console.WriteLine("value of ou: " + options.OU);
            }

            if(options.GetContractors)
            {
                ADDomain domain = new ADDomain();
                List<UserPrincipalEx> users = domain.GetAllContractors();

                foreach(var user in users)
                {
                  
                 Console.WriteLine(user);
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
    }
}
