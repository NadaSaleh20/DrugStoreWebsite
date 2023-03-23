using DataBase;
using DataBase.Data;
using DataBase.Dtos;
using DataBase.Models;
using DataBase.ViewModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Interfaces;
namespace DataAccess.Repository.Implementation
{
    public class OrderMangment : IOrderMangment
    {
        private readonly ApplicationDBContext _context;

        public OrderMangment(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(string PharmcyId, DrugInfoObjectRows createOrders)
        {
            var FinalOrder = new FinalOrder();
            var RowOrder = new OrderItem();
            var RoWOrderList = new List<OrderItem>();
            var Pharmcy = _context.Users.Where(x => x.Id == PharmcyId).FirstOrDefault();

            float TotalPriceFinal = 0;

            //Remove InAvalibe Element 
            createOrders.DrugInfoObjectRow.RemoveAll(x => !x.IsAvialable);
            //if it have element After i delete it 
            if (createOrders.DrugInfoObjectRow.Count() > 0)
            {
                foreach (var order in createOrders.DrugInfoObjectRow)
                {
                    TotalPriceFinal += order.TotalPriceRow;
                    //Discount The Quantity Storge  in DrugTabel , Save it in the DB
                    var Drug = _context.Drug.Where(x => x.DrugId == order.DrugID).FirstOrDefault();
                    Drug.QuantityStorage -= order.Quantity;

                    RoWOrderList.Add(new OrderItem { Drug = Drug, QunatityOrder = order.Quantity, Status = DataBase.Helper.OrderStatus.pinding });
                }
                FinalOrder.pharmcyInfo = Pharmcy;
                FinalOrder.TotalPrice = TotalPriceFinal;
                FinalOrder.Date = DateTime.Now;

                foreach (var item in RoWOrderList)
                {
                    item.FinalOrder = FinalOrder;
                }
                _context.OrderItem.AddRange(RoWOrderList);
                await _context.finalOrders.AddAsync(FinalOrder);
                await _context.SaveChangesAsync();

            }
        }


        //Display Current Order
        public async Task<CurrentOrdersVM> GetCurrentOrderAsync(string PharmcyId, int start, int length)
        {
            var currentOrdersQ = _context.OrderItem.Include(x => x.Drug).Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo)
              .Where(x => x.FinalOrder.pharmcyInfo.Id.Contains(PharmcyId) && x.Status == DataBase.Helper.OrderStatus.pinding).OrderByDescending(x => x.FinalOrder.Date).AsQueryable();

            var total = currentOrdersQ.Count();
            currentOrdersQ = currentOrdersQ.Skip(start).Take(length);

            var orders = currentOrdersQ.Select(x => new GetCurrentOrderDtos
            {
                OrderId = x.FinalOrder.orderItems.SingleOrDefault(y => y.Drug.DrugId == x.Drug.DrugId).Id,
                DrugID = x.Drug.DrugId,
                DrugName = x.Drug.DrugName,
                Quantity = x.QunatityOrder,
                IsAvialable = true,
                pricePerUnit = x.Drug.Price,
                TotalPriceRow = x.Drug.Price * x.QunatityOrder,
                Status = x.Status,
                DateString = x.FinalOrder.Date.ToString("MM/dd/yyyy HH:mm:ss"),
                TotalPrice = x.FinalOrder.TotalPrice,
                finalOrderId = x.FinalOrder.Id

            }).ToList();

            return new CurrentOrdersVM { Count = total, Orders = orders, FilteredCount = orders.Count };
        }


        //DisplayArchivedOrder

        public async Task<CurrentOrdersVM> GetArichviedOrderAsync(string PharmcyId, int start, int length)
        {
            var currentOrdersQ = _context.OrderItem.Include(x => x.Drug).Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo)
              .Where(x => x.FinalOrder.pharmcyInfo.Id.Contains(PharmcyId) && x.Status != DataBase.Helper.OrderStatus.pinding)
              .OrderByDescending(x => x.FinalOrder.Date).AsQueryable();

            var total = currentOrdersQ.Count();
            currentOrdersQ = currentOrdersQ.Skip(start).Take(length);
            var orders = currentOrdersQ.Select(x => new GetCurrentOrderDtos
            {
                OrderId = x.FinalOrder.Id,
                Status = x.Status,
                DrugID = x.Drug.DrugId,
                DrugName = x.Drug.DrugName,
                Quantity = x.QunatityOrder,
                pricePerUnit = x.Drug.Price,
                TotalPriceRow = x.Drug.Price * x.QunatityOrder,
                IsAvialable = true,
                DateString = x.FinalOrder.Date.ToString("MM/dd/yyyy HH:mm:ss"),
                TotalPrice = x.FinalOrder.TotalPrice
            }).ToList();
            return new CurrentOrdersVM { Count = total, Orders = orders, FilteredCount = orders.Count };
        }


        //test this is not completed
        public async Task<DeleteOrderDtos> DeleteOrder(string PharmcyID, int OrderID)
        {
            var RowWantDeleted = _context.OrderItem.Include(x => x.Drug).Include(x => x.FinalOrder)
                   .SingleOrDefault(x => x.Status == DataBase.Helper.OrderStatus.pinding
                     && x.Id == OrderID && x.FinalOrder.pharmcyInfo.Id == PharmcyID
                   );
            if (RowWantDeleted != null)
            {
                RowWantDeleted.Status = DataBase.Helper.OrderStatus.Canseled;

                //return Qunatity to Storge
                var drug = _context.Drug.SingleOrDefault(x => x.DrugId == RowWantDeleted.Drug.DrugId);
                drug.QuantityStorage += RowWantDeleted.QunatityOrder;

                //sub this Price From TotalPrice 
                var PriceForRowWantTodeleted = RowWantDeleted.QunatityOrder * RowWantDeleted.Drug.Price;
                var TotalPriceAfterDeleted = RowWantDeleted.FinalOrder.TotalPrice - PriceForRowWantTodeleted;
                RowWantDeleted.FinalOrder.TotalPrice = TotalPriceAfterDeleted;

                //we want to update the value of this Row in Tabel Orderitem Status
                //update Drug tabel => QuantityStorge
                // update finalOrderTabel
                _context.OrderItem.Update(RowWantDeleted);
                _context.Drug.Update(drug);
                await _context.SaveChangesAsync();
                return new DeleteOrderDtos
                {
                    DrugID = RowWantDeleted.Drug.DrugId,
                    DrugName = RowWantDeleted.Drug.DrugName,
                    IsAvialable = true,
                    pricePerUnit = RowWantDeleted.Drug.Price,
                    Quantity = RowWantDeleted.QunatityOrder,
                    TotalPriceRow = RowWantDeleted.Drug.Price * RowWantDeleted.QunatityOrder,
                    Status = RowWantDeleted.Status
                };
            }
            return null;
        }

        public async Task<List<DrugDeitlesDtos>> GetsDrugsFromDB()
        {
            return await _context.Drug.Select(x => new DrugDeitlesDtos { DrugId = x.DrugId, DrugName = x.DrugName }).ToListAsync();
        }


        public async Task<CreateOrderDtos> createRowOrder(AddDrugDtos drug)
        {
            var signalRowOrder = new CreateOrderDtos();
            var Drug = _context.Drug.SingleOrDefault(x => x.DrugId == drug.DrugId);

            signalRowOrder.DrugID = Drug.DrugId;
            signalRowOrder.DrugName = Drug.DrugName;
            signalRowOrder.Quantity = drug.DrugQuantity;

            if (drug.DrugQuantity <= 0 || drug.DrugQuantity > Drug.QuantityStorage)
            {
                signalRowOrder.IsAvialable = false;
                return signalRowOrder;
            }
            signalRowOrder.IsAvialable = true;
            signalRowOrder.pricePerUnit = Drug.Price;
            signalRowOrder.TotalPriceRow = Drug.Price * drug.DrugQuantity;
            return signalRowOrder;


        }

        public async Task<List<CurrentOrderAdmin>> OrdersBeEdits(OrdersToBeEdits orders)
        {
            return await _context.OrderItem.Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo)
                 .Where(x => x.FinalOrder.pharmcyInfo.Id == orders.pharmcyName && x.FinalOrder.Id == orders.finalOrderId
                 && x.Status == DataBase.Helper.OrderStatus.pinding).
                 Select(x => new CurrentOrderAdmin
                 {
                     DrugID = x.Drug.DrugId,
                     DrugName = x.Drug.DrugName,
                     Quantity = x.QunatityOrder,
                     pricePerUnit = x.Drug.Price,
                     TotalPriceRow = x.Drug.Price * x.QunatityOrder,
                     OrderId = x.Id,
                     pharmcyName = x.FinalOrder.pharmcyInfo.UserName,
                     FInalOrderId = x.FinalOrder.Id

                 }).ToListAsync();
        }
        public async Task<Drug> GetspecificDrug(int drugId)
        {
            var Drug = await _context.Drug.SingleOrDefaultAsync(x => x.DrugId == drugId);
            return new Drug
            {
                DrugName = Drug.DrugName,
                DrugId = Drug.DrugId,
                Price = Drug.Price,
                QuantityStorage = Drug.QuantityStorage
            };

        }
        public async Task<int> UpdateOrders(List<UpdateOrderVM> UpdatesOrdes)
        {
            //flag 0 = No element in list To Updated
            //flag 1 = Quantity New > Quantity Storge
            //flag 2 = edit Suceesfully Done
            try
            {
                var flag = 0;

                if (UpdatesOrdes.Count > 0)
                {
                    foreach (var order in UpdatesOrdes)
                    {
                        var finalOrder = await _context.finalOrders.SingleOrDefaultAsync(x => x.Id == order.finalOrderId);

                        var orderToBeEdits = await _context.OrderItem.Include(x => x.Drug).Include(x => x.FinalOrder).ThenInclude(x => x.pharmcyInfo)
                        .SingleOrDefaultAsync(x => x.Status == DataBase.Helper.OrderStatus.pinding
                        && x.FinalOrder.pharmcyInfo.Id == order.pharmcyName
                        && x.Id == order.orderId
                         //&& x.Drug.DrugId == order.drugId
                         );

                        var Drug = await _context.Drug.SingleOrDefaultAsync(x => x.DrugId == order.drugId);


                        var OldQuantity = orderToBeEdits.QunatityOrder;
                        var newQuantity = order.QuaNew;
                        var Diffrence = newQuantity - OldQuantity;
                        var oldorderprice = OldQuantity * orderToBeEdits.Drug.Price;
                        var neworderPrice = newQuantity * Drug.Price;
                        var oldTotalprice = finalOrder.TotalPrice;
                        var DiffrencePrice = 0;

                        //increse Quantity 
                        if (Diffrence > 0)
                        {
                            if (Diffrence <= Drug.QuantityStorage)
                            {
                                Drug.QuantityStorage -= Diffrence;
                                orderToBeEdits.QunatityOrder = newQuantity;
                                DiffrencePrice = neworderPrice - oldorderprice;
                                finalOrder.TotalPrice = oldTotalprice + DiffrencePrice;
                                orderToBeEdits.Drug = null;
                                orderToBeEdits.Drug = Drug;
                                _context.Drug.Update(Drug);
                                _context.OrderItem.Update(orderToBeEdits);
                                _context.finalOrders.Update(finalOrder);
                                await _context.SaveChangesAsync();
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
                            orderToBeEdits.Drug = null;
                            orderToBeEdits.Drug = Drug;
                            //orderToBeEdits.Drug.DrugName = Drug.DrugName;
                            _context.Drug.Update(Drug);
                            _context.OrderItem.Update(orderToBeEdits);
                            _context.finalOrders.Update(finalOrder);
                            await _context.SaveChangesAsync();
                            flag = 2;

                        }
                    }
                }

                else
                {
                    flag = 0;
                }

                return flag;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return 0;
            }
        }
    }
}
