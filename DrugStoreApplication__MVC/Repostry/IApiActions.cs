using System.Net.Http;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Repostry
{
    public interface IApiActions
    {
        Task<string> PostToAPi(string path, object obj);
       Task<string> GetFromApi(string path);
        Task<string> DeleteFromApi(string path);

    }
}