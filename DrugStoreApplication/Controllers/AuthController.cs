using DataBase.Models;
//using DrugStoreApplication.Repostry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Interfaces;

namespace DrugStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorization _authorization;

        public AuthController(IAuthorization authorization)
        {
          _authorization = authorization;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddPharmcy")]
       public async Task<IActionResult> AddNewPharmcyAsync([FromBody]AddPharmcyModel PharmcyModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authorization.AddPharmcyAsync(PharmcyModel);

            if (!result.IsAuthntecated)
                return BadRequest(result.Message);

            return Ok(new {Rules = result.Rules , PharmcyNum = PharmcyModel.AccountNum , Password = PharmcyModel.Password , PharmcyName = PharmcyModel.PharmcyName });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginPharmcy loginobj)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authorization.Login(loginobj);

            if (!result.IsAuthntecated)
                return BadRequest(result.Message);

            return Ok(new { Rule = result.Rules , Token = result.Token });

        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var result = await _authorization.Profile();
            return Ok(result);
        }
      


    }
}
