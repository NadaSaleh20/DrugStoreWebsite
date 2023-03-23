using DataBase;
using DataBase.Models;
using DrugStoreApplication__MVC.Models;
using DrugStoreApplication__MVC.Repostry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminMangment _adminMangment;
        private readonly IApiActions _apiActions;

        public AdminController(IAdminMangment adminMangment , IApiActions apiActions)
        {
            _adminMangment = adminMangment;
            _apiActions = apiActions;
        }
        [HttpPost]
        public async Task<IActionResult>UploadDrug(IFormFile postedFile)
        {
           await _adminMangment.ImportDrugsToDataBase(postedFile);
            return RedirectToAction("index", "home");

        }


        public IActionResult PharmcyiesPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadPharmcyies(IFormFile postedFile)
        {
           await _adminMangment.ImportPharmciesToDataBase(postedFile);
            return RedirectToAction("PharmcyiesPage", "Admin");
            
        }

        public async Task<IActionResult> GetPharmcies(int start, int length)
        {
          var result = await _apiActions.GetFromApi($"api/Admin/GetPharmices?start={start}&length={length}");
            var obj = JsonConvert.DeserializeObject<GetPharmcyiesPaging>(result);
            return Json(obj.Customers);

        }


        public IActionResult CurrentOrderAdmin()
        {
            return View();
        }

        public async Task<IActionResult> GetCurrentOrderAtAdmin(CurentOrderAdminVM curentOrderAdminVM)
        {
            var result = await _apiActions.PostToAPi("api/Admin/GetCurrentOrdersAdmin" , curentOrderAdminVM);
            var obj = JsonConvert.DeserializeObject<CurrentOrderAdminVM>(result);
            return Json(obj.Orders);

        }


        public async Task<IActionResult> ConfirmOrders(int finalOrderId, string PharmcyName)
        {

            var obj = new ConfirmOrder
            {  
             FinalOrderId = finalOrderId  ,
             PharmcyName= PharmcyName
            };

            await _apiActions.PostToAPi("api/Admin/ConfirmOrders" , obj);
            return Ok();

        }
        public IActionResult ArchivedOrderAdmin()
        {

            return View();
        }

        public async Task<IActionResult> GetArchivedOrderAdmin(CurentOrderAdminVM curentOrderAdminVM)
        {
            var result = await _apiActions.PostToAPi("api/Admin/GetArchviedOrders" , curentOrderAdminVM);
            var obj = JsonConvert.DeserializeObject<CurrentOrderAdminVM>(result);
            return Json(obj.Orders);
        }

        public async Task<IActionResult> SelectPharmcies()
        {
            var result = await _apiActions.GetFromApi($"api/Admin/PharmcySelect2");
            var obj = JsonConvert.DeserializeObject<List<Customer>>(result);
            return Json(obj);


        }

        public async Task<IActionResult> GetOrdersBeEditedByAdmin(int finalOrderId, string pharmcyName)
        {
            var obj = new OrdersToBeEdits { finalOrderId = finalOrderId, pharmcyName = pharmcyName };
            var result = await _apiActions.PostToAPi("api/Admin/GetOrdersBeEdited", obj);
            var orders = JsonConvert.DeserializeObject<List<CurrentOrderAdmin>>(result);
            return Json(orders);
             
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrdres([FromBody]List<UpdateOrderVM> UpdatesOrdes)
        {
            var result = await _apiActions.PostToAPi("api/Admin/UpdateOrders", UpdatesOrdes);
            return Json(result);
        }
    }
}
