using DataBase;
using DataBase.Data;
using DataBase.Models;
using DrugStoreApplication__MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Interfaces;

namespace DataAccess.Repository.Implementation
{
    public class AdminRepostry : IAdminRepostry
    {
        private readonly ApplicationDBContext _context;

        public AdminRepostry(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<GetPharmcyiesPaging> GetPharmcies(int start, int length)
        {
            var pharmcyInfo = _context.Users.Select(x => new Customer { AccountNumber = x.AccountNum, PharmcyName = x.UserName }).AsQueryable();
            var total = pharmcyInfo.Count();
            pharmcyInfo = pharmcyInfo.Skip(start).Take(length);
            var customers = pharmcyInfo.Select(x => new Customer { AccountNumber = x.AccountNumber, PharmcyName = x.PharmcyName }).ToList();

            return new GetPharmcyiesPaging { Count = total, FilterdCount = pharmcyInfo.Count(), Customers = customers };

        }


        public async Task<CurrentOrderAdminVM> GetCurrentOrderAdmin(CurentOrderAdminVM curentOrderAdminVM)
        {

            var AllOrdersQ = _context.OrderItem.Include(x => x.Drug).Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo)
                 .Where(x => x.Status == DataBase.Helper.OrderStatus.pinding)
                 .Where(x => curentOrderAdminVM.drugId.Value == 0 ? true : x.Drug.DrugId == curentOrderAdminVM.drugId)
                .Where(x => curentOrderAdminVM.pharmcyId == null ? true : x.FinalOrder.pharmcyInfo.Id == curentOrderAdminVM.pharmcyId)
                .Where(x => (curentOrderAdminVM.MinPrice.Value == 0 && curentOrderAdminVM.MaxPrice.Value == 0) ? true : x.FinalOrder.TotalPrice >= curentOrderAdminVM.MinPrice && x.FinalOrder.TotalPrice <= curentOrderAdminVM.MaxPrice)
                .Where(x => (curentOrderAdminVM.minDate == null && curentOrderAdminVM.maxDate == null) ? true : x.FinalOrder.Date >= curentOrderAdminVM.minDate && x.FinalOrder.Date <= curentOrderAdminVM.maxDate)
               .OrderByDescending(x => x.FinalOrder.Date).AsQueryable();
           

            var total = AllOrdersQ.Count();
            AllOrdersQ = AllOrdersQ.Skip(curentOrderAdminVM.start).Take(curentOrderAdminVM.length);


            var orders = AllOrdersQ.Select(
             x => new CurrentOrderAdmin
             {
                 DateString = x.FinalOrder.Date.ToString("MM/dd/yyyy HH:mm:ss"),
                 DrugID = x.Drug.DrugId,
                 DrugName = x.Drug.DrugName,
                 pharmcyName = x.FinalOrder.pharmcyInfo.UserName,
                 pricePerUnit = x.Drug.Price,
                 Quantity = x.QunatityOrder,
                 TotalPriceRow = x.QunatityOrder * x.Drug.Price,
                 TotalPrice = x.FinalOrder.TotalPrice,
                 FInalOrderId = x.FinalOrder.Id

             }).ToList();

            return new CurrentOrderAdminVM { Count = total, Orders = orders, FilteredCount = orders.Count };


        }


        public async Task ConfirmOrder(int finalOrderId, string PharmcyName)
        {
            var orders = _context.OrderItem.Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo)
            .Where(x => x.FinalOrder.Id == finalOrderId && x.FinalOrder.pharmcyInfo.UserName == PharmcyName && x.Status == DataBase.Helper.OrderStatus.pinding).ToList();

            //orders.RemoveAll(x => x.Status == DataBase.Helper.OrderStatus.Canseled);
            foreach (var order in orders)
            {
                order.Status = DataBase.Helper.OrderStatus.Complete;
            }

            _context.OrderItem.UpdateRange(orders);
            await _context.SaveChangesAsync();
        }

        public async Task<CurrentOrderAdminVM> GetArcivrdOrderAdmin(CurentOrderAdminVM curentOrderAdminVM)
        {
            var AllOrdersQ = _context.OrderItem.Include(x => x.Drug).Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo).
                Where(x => x.Status != DataBase.Helper.OrderStatus.pinding)
                .Where(x => curentOrderAdminVM.drugId.Value == 0 ? true : x.Drug.DrugId == curentOrderAdminVM.drugId)
                .Where(x => curentOrderAdminVM.pharmcyId == null ? true : x.FinalOrder.pharmcyInfo.Id == curentOrderAdminVM.pharmcyId)
                .Where(x => (curentOrderAdminVM.MinPrice.Value == 0 && curentOrderAdminVM.MaxPrice.Value == 0) ? true : x.FinalOrder.TotalPrice >= curentOrderAdminVM.MinPrice && x.FinalOrder.TotalPrice <= curentOrderAdminVM.MaxPrice)
                .Where(x => (curentOrderAdminVM.minDate == null && curentOrderAdminVM.maxDate == null) ? true : x.FinalOrder.Date >= curentOrderAdminVM.minDate && x.FinalOrder.Date <= curentOrderAdminVM.maxDate)
               .OrderByDescending(x => x.FinalOrder.Date).AsQueryable();

            var total = AllOrdersQ.Count();
            AllOrdersQ = AllOrdersQ.Skip(curentOrderAdminVM.start).Take(curentOrderAdminVM.length);

            var orders = AllOrdersQ.Select(
                        x => new CurrentOrderAdmin
                        {
                            DateString = x.FinalOrder.Date.ToString("MM/dd/yyyy HH:mm:ss"),
                            DrugID = x.Drug.DrugId,
                            DrugName = x.Drug.DrugName,
                            pharmcyName = x.FinalOrder.pharmcyInfo.UserName,
                            pricePerUnit = x.Drug.Price,
                            Quantity = x.QunatityOrder,
                            TotalPriceRow = x.QunatityOrder * x.Drug.Price,
                            TotalPrice = x.FinalOrder.TotalPrice,
                            orderStatus = x.Status


                        }
                ).ToList();

            return new CurrentOrderAdminVM { Count = total, Orders = orders, FilteredCount = orders.Count };
        }

        public async Task<List<Customer>> GetPharmciesSelect2()
        {
            return await _context.Users.Select(x => new Customer {  pharmcyId = x.Id, PharmcyName = x.UserName }).ToListAsync();

        }

        public async Task<List<CurrentOrderAdmin>> OrdersBeEdits(OrdersToBeEdits orders)
        {
           return await _context.OrderItem.Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo)
                .Where(x => x.FinalOrder.pharmcyInfo.UserName == orders.pharmcyName && x.FinalOrder.Id == orders.finalOrderId
                && x.Status == DataBase.Helper.OrderStatus.pinding).
                Select(x => new CurrentOrderAdmin
                {
                     DrugID = x.Drug.DrugId,
                      DrugName = x.Drug.DrugName,
                       Quantity = x.QunatityOrder,
                        pricePerUnit = x.Drug.Price ,
                         TotalPriceRow = x.Drug.Price * x.QunatityOrder,
                         OrderId = x.Id,
                          pharmcyName = x.FinalOrder.pharmcyInfo.UserName,
                          FInalOrderId = x.FinalOrder.Id
                          
                }).ToListAsync();
        }

        public async Task<int> UpdateOrders(List<UpdateOrderVM> UpdatesOrdes)
        {
            //flag 0 = No element in list To Updated
            //flag 1 = Quantity New > Quantity Storge
            //flag 2 = edit Suceesfully Done
            var flag = 0;

            if (UpdatesOrdes.Count > 0)
            {
                foreach (var order in UpdatesOrdes)
                {
                    var finalOrder = await _context.finalOrders.SingleOrDefaultAsync(x => x.Id == order.finalOrderId);

                    var orderToBeEdits = await _context.OrderItem.Include(x => x.Drug).Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo)
                    .SingleOrDefaultAsync(x => x.Status == DataBase.Helper.OrderStatus.pinding
                    && x.FinalOrder.pharmcyInfo.UserName == order.pharmcyName
                    && x.Id == order.orderId
                    && x.Drug.DrugId == order.drugId
                     );
                    var Drug = await _context.Drug.SingleOrDefaultAsync(x => x.DrugId == order.drugId);

                    var OldQuantity = orderToBeEdits.QunatityOrder;
                    var newQuantity = order.QuaNew;
                    var Diffrence = newQuantity - OldQuantity;
                    var oldorderprice = OldQuantity * Drug.Price;
                    var neworderPrice = newQuantity * Drug.Price;
                    var oldTotalprice = finalOrder.TotalPrice;
                    var DiffrencePrice = 0;
                    //increse Quantity 
                    if (Diffrence > 0 )
                    {
                        if (Diffrence <= Drug.QuantityStorage)
                        {
                            Drug.QuantityStorage -= Diffrence;
                            orderToBeEdits.QunatityOrder = newQuantity;
                            DiffrencePrice = neworderPrice - oldorderprice;
                            finalOrder.TotalPrice = oldTotalprice + DiffrencePrice;
                            _context.Drug.Update(Drug);
                            _context.OrderItem.Update(orderToBeEdits);
                            _context.finalOrders.Update(finalOrder);
                           flag = 2;
                        }
                        else
                        {
                            flag = 1;
                        }
                     
                    }
                    //decrease Quantity
                    else
                    {
                        Drug.QuantityStorage += Diffrence;
                        orderToBeEdits.QunatityOrder = newQuantity;
                        DiffrencePrice = oldorderprice - neworderPrice;
                        finalOrder.TotalPrice = oldTotalprice - DiffrencePrice;
                        _context.Drug.Update(Drug);
                        _context.OrderItem.Update(orderToBeEdits);
                        _context.finalOrders.Update(finalOrder);
                      
                        flag = 2;

                    }
                }
            }
            else
            {
                flag = 0;
            }
            await _context.SaveChangesAsync();
            return flag;
        }
    }
    }
