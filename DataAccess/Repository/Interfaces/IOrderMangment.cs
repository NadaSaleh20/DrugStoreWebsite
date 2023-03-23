
using DataBase;
using DataBase.Data;
using DataBase.Dtos;
using DataBase.Models;
using DataBase.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interfaces
{
    public interface IOrderMangment
    {
        Task CreateOrder(string PharmcyId, DrugInfoObjectRows createOrders);
        Task<CurrentOrdersVM> GetCurrentOrderAsync(string PharmcyId, int start, int length);

        Task<CurrentOrdersVM> GetArichviedOrderAsync(string PharmcyId, int start, int length);
        Task<DeleteOrderDtos> DeleteOrder(string PharmcyID, int OrderID);
        Task<List<DrugDeitlesDtos>> GetsDrugsFromDB();

        Task<CreateOrderDtos> createRowOrder(AddDrugDtos drug);
        Task<List<CurrentOrderAdmin>> OrdersBeEdits(OrdersToBeEdits orders);
        Task<Drug> GetspecificDrug(int drugId);
        Task<int> UpdateOrders(List<UpdateOrderVM> UpdatesOrdes);
    }
}