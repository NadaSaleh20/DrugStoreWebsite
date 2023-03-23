
using DataBase.Data;
using DataBase.ViewModel;
using DrugStoreApplication__MVC.Models;
using DrugStoreApplication__MVC.Repostry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAccount _accountRepostry;
        private readonly IApiActions _apiActions;
        private readonly SignInManager<PharmcyInfo> _signInManager;
        private readonly UserManager<PharmcyInfo> _userManager;
        
        
        public HomeController(ILogger<HomeController> logger , IAccount accountRepostry , IApiActions apiActions , SignInManager<PharmcyInfo> signInManager , UserManager<PharmcyInfo> UserManager)
        {
            _logger = logger;
            _accountRepostry = accountRepostry;
           _apiActions = apiActions;
          _signInManager = signInManager;
            _userManager = UserManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPage(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                if (await _accountRepostry.Login(model))
                {
                    //get the token from Api and store it in Cookiee
                   var result = await _apiActions.PostToAPi("api/Auth/Login" , new { accountNum = model.PharmcyCode, password = model.PharmcyPassword });
                    if (!String.IsNullOrEmpty(result))
                    {
                        var obj = JsonConvert.DeserializeObject<LoginResultFromApi>(result);
                        Response.Cookies.Append("token" , obj.Token);
                    }
                    //Redirect To Home Page
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("" , "Invaild Login"); 
            }
             return View();
            }


        public async Task <IActionResult> SiginOut()
        {
            await _signInManager.SignOutAsync();
            //delete token From the Cookies
            Response.Cookies.Delete("token");
            return RedirectToAction("index", "Home");
        }


        [Authorize(Roles = "User")]
        public async Task <IActionResult> PharmcyInfo()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangePharmcyName( string PharmcyName )
        {
            //Get The id of the Pharmcy Login
            var user = _userManager.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var PharmcyId = user.Id;
            var result =await _apiActions.PostToAPi("api/PharmcyInformation/ChangePharmcyName" , new {
                PharmcyId = PharmcyId,
                newPharmcyName = PharmcyName
            });
            user.UserName = PharmcyName;
            await _signInManager.RefreshSignInAsync(user);
            return Ok(result);
          
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangePassword( string currentPassword , string newPassword )
        {
            var user = _userManager.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var PharmcyId = user.Id;
            var result = await _apiActions.PostToAPi("api/PharmcyInformation/ChangePassword", new
            {
                PharmcyId = PharmcyId,
                password = currentPassword,
                newPassword = newPassword
            });
            return Ok(result);
        }

        public async Task<IActionResult> GetDrugs()
        {

            var result = await _apiActions.GetFromApi("api/Order");
            var obj = JsonConvert.DeserializeObject<List<DrugInfo>>(result);
            return Json(obj);

        }

    }
}
