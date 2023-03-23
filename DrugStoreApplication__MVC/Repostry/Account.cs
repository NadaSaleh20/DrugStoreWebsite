using DataBase.Data;
using DrugStoreApplication__MVC.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Repostry
{
    public class Account : IAccount
    {
        private readonly UserManager<PharmcyInfo> _userManager;
        private readonly SignInManager<PharmcyInfo> _signInManager;

        public Account(UserManager<PharmcyInfo> userManager ,SignInManager<PharmcyInfo> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> Login(LoginModel model)
        {
            try
            {
                var PharmcyCode = _userManager.Users.SingleOrDefault(x => x.AccountNum == model.PharmcyCode);
                var result = await _signInManager.PasswordSignInAsync(PharmcyCode, model.PharmcyPassword, model.RembmerMe, true);
                return result.Succeeded;
            }
            //if result not Succeed return false 
            catch (Exception)
            {

                return false;
            }
          
      
        }
    }
}
