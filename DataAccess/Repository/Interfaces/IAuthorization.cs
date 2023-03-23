using DataBase.Data;
using DataBase.Models;
using DataBase.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interfaces
{
    public interface IAuthorization
    {
        Task<AuthModel> AddPharmcyAsync(AddPharmcyModel model);
        Task<AuthModel> Login(LoginPharmcy loginpharmcy);
        Task<PharmcyInfoViewModel> Profile();
    }
}