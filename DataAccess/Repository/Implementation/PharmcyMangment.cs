using DataBase.Data;
using DataBase.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Interfaces;
using DataBase.Dtos;

namespace DataAccess.Repository.Implementation
{
    public class PharmcyMangment : IPharmcyMangment
    {
        private readonly ApplicationDBContext _Context;
        private readonly UserManager<PharmcyInfo> _userManager;

        public PharmcyMangment(ApplicationDBContext Context, UserManager<PharmcyInfo> userManager)
        {
            _Context = Context;
            _userManager = userManager;
        }

        public async Task<PharmcyInformationDeitles> GetPharmcyDEitels(string PharmcyId)
        {
            return _Context.Users.Where(x => x.Id == PharmcyId)
                 .Select(x => new PharmcyInformationDeitles
                 {
                    //AccountNumber = x.AccountNum,
                    PharmcyName = x.UserName,
                 }
                 ).FirstOrDefault();
        }

        //Change Pharmcy Name 
        public async Task<PharmcyInformationDeitles> ChangePharmcyName(string PharmcyId, string PharmcyName)
        {
            var Pharmcy = _Context.Users.Where(x => x.Id == PharmcyId).FirstOrDefault();

            if (PharmcyName != null)
            {
                Pharmcy.UserName = PharmcyName;
                _Context.Users.Update(Pharmcy);
                await _Context.SaveChangesAsync();
            }

            return new PharmcyInformationDeitles
            {
                PharmcyName = Pharmcy.UserName,
            };
        }


        //change PharmcyPassword
        public async Task<bool> ChangePharmcyPassword(string PharmcyId, string password, string newPassword)
        {
            var Pharmcy = _Context.Users.Where(x => x.Id == PharmcyId).FirstOrDefault();
            var result = await _userManager.ChangePasswordAsync(Pharmcy, password, newPassword);
            return result.Succeeded;
        }
    }
}
