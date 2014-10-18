using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            ADDomain domain = new ADDomain();
            List<UserPrincipal> users = domain.GetAllContractors();

            foreach(var user in users)
            {
                Console.WriteLine(user.SamAccountName + " -> " + user.GivenName + " " + user.Surname);
            }

            Console.WriteLine("Results found: " + users.Count);

            Console.ReadLine();
        }
    }
}
