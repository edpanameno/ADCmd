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
            /*ADDomain domain = new ADDomain();
            List<UserPrincipalEx> users = domain.GetAllContractors();

            foreach(var user in users)
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("Results found: " + users.Count);*/
            var options = new ProgramOptions();

            string ouContainer = string.Empty;
            var p = new OptionSet()
            {
                {"o", a => options.OU = a}
            };

            p.Parse(args);

            //p.WriteOptionDescriptions(Console.Out);

            if(options.OU != null)
            {
                Console.WriteLine("OU: " + options.OU);
            }

            //Console.ReadLine();
        }
    }

    /// <summary>
    /// This class holds the different options that can be used when running
    /// this utility from the command line.
    /// </summary>
    public class ProgramOptions
    {
        public string OU { get; set; }
    }
}
