using DrugStoreApplication__MVC.Models;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Repostry
{
    public interface IAccount
    {
        Task<bool> Login(LoginModel model);
    }
}