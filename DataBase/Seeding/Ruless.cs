using DataBase.Data;
using DataBase.Helper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Seeding
{
    public class Ruless
    {
        public static async Task AddRuleAsync(RoleManager<IdentityRole> roleManger)
        {
            //Make Sure That RoleManger Doesnt Have Exist Role Yet
            if (!roleManger.Roles.Any())
            {
                await roleManger.CreateAsync(new IdentityRole(Rules.Admin.ToString()));
                await roleManger.CreateAsync(new IdentityRole(Rules.User.ToString()));
            }
        }

        public static async Task AddAdmin(UserManager<PharmcyInfo> userManager)
        {
            var newUser = new PharmcyInfo
            {
                UserName = "Nada",
                AccountNum =2000

            };
            var user =  userManager.Users.Where(x => x.AccountNum == newUser.AccountNum).FirstOrDefault();

            //if not exit Create it 
            if (user == null)
            {
                //create it with userName , Password
                await userManager.CreateAsync(newUser, "Ap@123456");

                //After Created it , it will assign the role to it 
                await userManager.AddToRoleAsync(newUser, Rules.Admin.ToString());

            }

        }
    }
}
