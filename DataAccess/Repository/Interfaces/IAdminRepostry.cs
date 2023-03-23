using DataBase;
using DataBase.Models;
using DrugStoreApplication__MVC.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interfaces
{
    public interface IAdminRepostry
    {
        Task<GetPharmcyiesPaging> GetPharmcies(int start, int length);
        Task<CurrentOrderAdminVM> GetCurrentOrderAdmin(CurentOrderAdminVM curentOrderAdminVM);
        Task ConfirmOrder(int finalOrderId, string PharmcyName);
        Task<CurrentOrderAdminVM> GetArcivrdOrderAdmin(CurentOrderAdminVM curentOrderAdminVM);
        Task<List<Customer>> GetPharmciesSelect2();
        Task<List<CurrentOrderAdmin>> OrdersBeEdits(OrdersToBeEdits orders);
        Task<int> UpdateOrders(List<UpdateOrderVM> UpdatesOrdes);
    }
}