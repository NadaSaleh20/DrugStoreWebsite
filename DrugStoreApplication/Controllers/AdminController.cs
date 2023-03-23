using DataBase;
using DataBase.Models;
//using DrugStoreApplicationApi.Repostry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Interfaces;
namespace DrugStoreApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepostry _adminRepostry;

        public AdminController(IAdminRepostry adminRepostry)
        {
            _adminRepostry = adminRepostry;
        }
        [HttpGet("GetPharmices")]
        public async Task<IActionResult> GetAllPharmices(int start, int length)
        {
            var obj = await _adminRepostry.GetPharmcies(start, length);
            return Ok(obj);
        }

        [HttpPost("GetCurrentOrdersAdmin")]
        public async Task<IActionResult> GetCurrentOrderAdmin(CurentOrderAdminVM curentOrderAdminVM)
        {
            var obj = await _adminRepostry.GetCurrentOrderAdmin(curentOrderAdminVM);
            return Ok(obj);

        }

        [HttpPost("ConfirmOrders")]
        public async Task<IActionResult> AdminConfirmOrders(ConfirmOrder confirmOrder)
        {
            await _adminRepostry.ConfirmOrder(confirmOrder.FinalOrderId, confirmOrder.PharmcyName);
            return Ok();
        }

        //[HttpPost("EditOrderAdmin")]
        //public async Task<IActionResult> EditOrders()
        //{

        //}

        [HttpPost("GetArchviedOrders")]
        public async Task<IActionResult> GetArchviedOrderAdmin(CurentOrderAdminVM curentOrderAdminVM)
        {
            var obj = await _adminRepostry.GetArcivrdOrderAdmin(curentOrderAdminVM);
            return Ok(obj);
        }

        [HttpGet("PharmcySelect2")]
        public async Task<IActionResult> GetPharmcySelect2()
        {
            var obj = await _adminRepostry.GetPharmciesSelect2();
            return Ok(obj);
        }
        [HttpPost("GetOrdersBeEdited")]
        public async Task<IActionResult> GetOrderEdit(OrdersToBeEdits orders)
        {
            var obj = await _adminRepostry.OrdersBeEdits(orders);
            return Ok(obj);
        }

        [HttpPost("UpdateOrders")]
        public async Task<IActionResult> UpdateOrders(List<UpdateOrderVM> UpdatesOrdes)
        {
            var obj = await _adminRepostry.UpdateOrders(UpdatesOrdes);
            return Ok(obj);
        }
    }
}
