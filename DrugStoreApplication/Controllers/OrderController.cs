using DataBase;
using DataBase.Data;
using DataBase.Models;
using DataBase.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Interfaces;
namespace DrugStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderMangment _orderMangment;

        public OrderController(IOrderMangment orderMangment)
        {
            _orderMangment = orderMangment;
        }
        //Create New Order 
        [HttpPost("CreateOrderByPharmcy")]
        
        public async Task<IActionResult> CreateOrderAsync(SaveRowsOrder  order)
        {
            await _orderMangment.CreateOrder(order.PharmcyID, order.rows);
            return Ok();
        }

        //Get The Current Order
        [HttpGet("GetAllCurrentOrder")]
        public async Task<IActionResult> GetCurrentOrder(string PharmcyID,  int start , int length)
        {
            var obj = await _orderMangment.GetCurrentOrderAsync(PharmcyID , start , length);
            return Ok(obj);
        }

        //Get the Archived Orderd
        [HttpGet("GetAllAricvedOrder")]
        public async Task<IActionResult> GetArchivedOrder(string PharmcyID , int start, int length)
        {
            var obj = await _orderMangment.GetArichviedOrderAsync(PharmcyID , start, length);
            return Ok(obj);
        }
         
        [HttpDelete]
        public async Task<IActionResult>DeleteOrderFromCurnnetOrder(string PharmcyID, int OrderID)
        {
            var obj = await _orderMangment.DeleteOrder(PharmcyID, OrderID);
            return Ok(obj);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDrugsFound()
        {
            var obj = await _orderMangment.GetsDrugsFromDB();
            return Ok(obj);
        }

        [HttpPost]
        public async Task<IActionResult> GetDrugDeitlesRow(AddDrugDtos model)
        {
            var obj = await _orderMangment.createRowOrder(model);
            return Ok(obj);

        }

        //Get Row Want To Update
        [HttpPost("GetOrdersBeEdited")]
        public async Task<IActionResult> GetOrderEdit(OrdersToBeEdits orders)
        {
            var obj = await _orderMangment.OrdersBeEdits(orders);
            return Ok(obj);
        }

        [HttpPost("GetspecificDrug")]
        public async Task<IActionResult> GetspecificDrug(Drug drug)
        {
            var obj = await _orderMangment.GetspecificDrug(drug.DrugId);
            return Ok(obj);

        }

        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrders(List<UpdateOrderVM> UpdatesOrdes)
        {
            var obj = await _orderMangment.UpdateOrders(UpdatesOrdes);
            return Ok(obj);
        }
    }
}
