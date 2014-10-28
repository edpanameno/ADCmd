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
                {"ou|organizationalUnit=", "Organizational Unit that holds the users", a => options.OU = a},
                {"dou|defaultOU=", "Get Users from the Organization Unit", a => options.GetDefaultOU = true},
                {"c|contractors=", "Get contractors from default OU", a => options.GetContractors = true },
                {"d|disabledUsers", "Get users who are disabled", a => options.GetDisabledUsers = true}
            };
            
            List<string> temp;
            temp = p.Parse(args);
            
            if(options.OU != null)
            {
                if(!domain.IsValidOU(options.OU))
                {
                    Console.WriteLine(options.OU + " is not a valid OU DN.");
                }
                else
                {
                    List<UserPrincipalEx> users = domain.GetUsersFromOU(options.OU);
                    
                    foreach(var user in users)
                    {
                        Console.WriteLine(user);
                    }
                }
            }

            if(options.GetContractors)
            {
                List<UserPrincipalEx> users = domain.GetAllContractors();

                foreach(var user in users)
                {
                    Console.WriteLine(user);
                }
            }

            if(options.GetDefaultOU)
            {
                List<UserPrincipalEx> users = domain.GetUsersFromOU(domain.DefaultOU);
                
                foreach(var user in users)
                {
                    Console.WriteLine(user);
                }
            }

            if(options.GetDisabledUsers)
            {
                Console.WriteLine("Inside of getdisabledusers");
                List<UserPrincipalEx> disabledUsers = domain.GetDisabledUsers();
                
                foreach(var user in disabledUsers)
                {
                    Console.WriteLine(user);
                }
            }
            
            if(temp != null)
            {
                foreach(var t in temp)
                {
                    Console.WriteLine("I am looping!");
                    Console.WriteLine(t);
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
        public bool GetDefaultOU { get; set; 
}
    }
}
