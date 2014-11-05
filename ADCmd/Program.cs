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
            List<ADUser> users = null;

            var p = new OptionSet()
            {
                {"o|organizationalUnit=", "Organizational Unit that holds the users", a => options.OU = a},
                {"c|contractors=", "Get contractors from default OU", a => options.GetContractors = true },
                {"d|disabledUsers", "Get users who are disabled", a => options.GetDisabledUsers = true},
                {"e|exportUsers", "Export Users to Excel File", a => options.ExportUsers = true},
                {"cu|createUser", "Create User", a => options.CreateUser = true},
                {"ag|addUserToGroup", "Add user to group", a => options.AddUsertoGroup = true},
                {"un|updateNotes", "Update the notes on a user", a => options.UpdateNotes = true},
                {"du|disableUser", "Disable user", a => options.DisableUser = true},
                {"gm|groupMembers", "Get members of a group", a => options.GroupMembers = true}
            }.Parse(args);

            if(options.GroupMembers)
            {
                Console.WriteLine("Inside of Groupmembers");
                Console.Write("Enter Group Name: ");
                string groupName = Console.ReadLine();
                domain.GetGroupMembers(groupName);
                
                return;
            }

            if(options.DisableUser)
            {
                Console.WriteLine("Inside of disabling user");
                Console.Write("Username to disable: ");
                string userID = Console.ReadLine();
                domain.DisableUser(userID);
                
                return;
            }

            if(options.UpdateNotes)
            {
                Console.WriteLine("Updating user notes");
                Console.Write("Enter username: ");
                string userName = Console.ReadLine();
                Console.Write("Notes: ");
                string notes = Console.ReadLine();
                domain.UpdateUserNotes(userName, notes);

                return;
            }

            if(options.AddUsertoGroup)
            {
                Console.WriteLine("Adding user to group");
                Console.Write("Enter username: ");
                string userName = Console.ReadLine();
                domain.AddUsertoGroup(userName, null);
                
                return;
            }

            if(options.CreateUser)
            {
                Console.WriteLine("Creating new user");
                domain.CreateUser();

                return;
            }
            
            if(String.IsNullOrEmpty(options.OU))
            {
                // Because no OU was passed, we are going to retrieve the list of users from
                // the default OU
                users = domain.GetUsersFromOU(domain.DefaultOU, options.GetDisabledUsers);

                foreach(var u in users)
                {
                    Console.WriteLine(u);
                }

                return;
            }
            else 
            {
                users = domain.GetUsersFromOU(options.OU, options.GetDisabledUsers);
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
        public bool CreateUser { get; set; }
        public bool AddUsertoGroup { get; set; }
        public bool UpdateNotes { get; set; }
        public bool DisableUser { get; set; }
        public bool GroupMembers { get; set; }
    }
}
