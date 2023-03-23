using DataBase;
using DataBase.Data;
using DataBase.Models;
using DataBase.ViewModel;
using DrugStoreApplication__MVC.Models;
using DrugStoreApplication__MVC.Repostry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Controllers
{
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private readonly IApiActions _apiActions;
        private readonly UserManager<PharmcyInfo> _userManager;
        private readonly ApplicationDBContext _context;

        public OrdersController(IApiActions apiActions , UserManager<PharmcyInfo> UserManager , ApplicationDBContext context)
        {
            _apiActions = apiActions;
            _userManager = UserManager;
            _context = context;
        }

        public IActionResult CreateOrder()
        {
            return View();
        }

    
        public async Task<IActionResult> GetDrugs()
        {

            var result = await _apiActions.GetFromApi("api/Order" );
            var obj = JsonConvert.DeserializeObject<List<DrugInfo>>(result);
            return Json(obj);
          
       }

       public async Task<IActionResult> GetRowDrugInfo(int drugId , int Qunantity)
        {
            var result = await _apiActions.PostToAPi("api/Order", new {
                drugId = drugId ,
                drugQuantity= Qunantity
            });
            var obj = JsonConvert.DeserializeObject<DrugInfoObjectRow>(result);
            return Json(obj);
        }

        public async Task<IActionResult> saveOrder([FromBody] DrugInfoObjectRows model)
        {

            //PharmcyId
            var user = _userManager.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var PharmcyId = user.Id;

            //Calling Api
            var tabel = new SaveRowsOrder {
                PharmcyID = PharmcyId,
                rows = model 
            };
            await _apiActions.PostToAPi("/api/Order/CreateOrderByPharmcy", tabel);
            return Ok();
        }

        // Current Order Page
        public IActionResult CurrentOrder()
        {
            return View();
        }

        // Archived Order Page

        public IActionResult AricvedOrder()
        {
            return View();
        }


        //Get Current Order From Api

        public async Task<IActionResult> GetCurrentOrder(int start, int length)
        {
            //PharmcyId
            var user = _userManager.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var PharmcyId = user.Id;

            //Calling the Api
           var result = await _apiActions.GetFromApi($"api/Order/GetAllCurrentOrder?PharmcyID={PharmcyId}&start={start}&length={length}");
            //Desrilaizing obj to list of GetCurrentOrderDtos
            var obj = JsonConvert.DeserializeObject<CurrentOrdersVM>(result); /*.OrderByDescending(x => x.DateString);*/
            //return Ok(new { draw = 1, recordsTotal = obj.Count, recordsFiltered = obj.Count, data = obj });
            return Ok(obj.Orders);

        }
        public async Task<IActionResult> GetArcviedOrder(int start, int length)
        {
            //PharmcyId
            var user = _userManager.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var PharmcyId = user.Id;

            //Calling the Api
            var result = await _apiActions.GetFromApi($"api/Order/GetAllAricvedOrder?PharmcyID={PharmcyId}&start={start}&length={length}");
            //Desrilaizing obj to list of GetCurrentOrderDtos
            var obj = JsonConvert.DeserializeObject<CurrentOrdersVM>(result); /*.OrderByDescending(x => x.DateString);*/
            //return Ok(new { draw = 1, recordsTotal = obj.Count, recordsFiltered = obj.Count, data = obj });
            return Ok(obj.Orders);

        }


        //Delete Order From Current Orders 
        public async Task<IActionResult> DeleteOrderFromCurrentOrders (int orderId)
        {
            //PharmcyId
            var user = _userManager.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var PharmcyId = user.Id;

            //Calling Api 
            var result = await _apiActions.DeleteFromApi($"api/Order?PharmcyID={PharmcyId}&OrderID={orderId}");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetOrdersBeEditedByPharmcy(int finalOrderId)
        {
            //PharmcyId
            var user = _userManager.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var PharmcyId = user.Id;
            var obj = new { finalOrderId = finalOrderId , pharmcyName = PharmcyId } ;
            var result = await _apiActions.PostToAPi("api/Order/GetOrdersBeEdited", obj);
            var orders = JsonConvert.DeserializeObject<List<CurrentOrderAdmin>>(result);
            return Json(orders);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrdres([FromBody] List<UpdateOrderVM> UpdatesOrdes)
        {
            var user = _userManager.Users.Where(x => x.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var PharmcyId = user.Id;
            UpdatesOrdes.ForEach(x => x.pharmcyName = PharmcyId);
            var result = await _apiActions.PostToAPi("api/Order/UpdateOrder", UpdatesOrdes);
            return Json(result);
        }

        
        public async Task<IActionResult> GetspecificDrug(int DrugIdUpdate)
        {
            var obj = new Drug { DrugId = DrugIdUpdate };
            var result = await _apiActions.PostToAPi("api/Order/GetspecificDrug", obj);
            return Json(result);
        }
    }
}
 