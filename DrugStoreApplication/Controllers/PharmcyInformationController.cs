using DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Interfaces;
using DataBase.Dtos;

namespace DrugStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class PharmcyInformationController : ControllerBase
    {
        private readonly IPharmcyMangment _pharmcyMangment;

        public PharmcyInformationController(IPharmcyMangment pharmcyMangment)
        {
         _pharmcyMangment = pharmcyMangment;
        }

        [HttpGet]
        public async Task<IActionResult> GetPharmcyDeitels(string PharmcyId)
        {
            var obj = await _pharmcyMangment.GetPharmcyDEitels(PharmcyId);
            return Ok(obj);
        }

        [HttpPost("ChangePharmcyName")]
        public async Task<IActionResult> ChangePharmcyName(ChangePharmcyNameAndCode model )
        {
            var obj = await _pharmcyMangment.ChangePharmcyName(model.PharmcyId, model.newPharmcyName );
            return Ok(obj);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> changePassword(ChangPassword changPassword)
        {
          var obj = await _pharmcyMangment.ChangePharmcyPassword(changPassword.PharmcyId, changPassword.Password , changPassword.NewPassword);
            return Ok(obj);
        }

    }
}