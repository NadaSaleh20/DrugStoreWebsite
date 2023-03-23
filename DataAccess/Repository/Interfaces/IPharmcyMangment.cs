using DataBase.Dtos;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interfaces
{
    public interface IPharmcyMangment
    {
        Task<PharmcyInformationDeitles> GetPharmcyDEitels(string PharmcyId);
        Task<PharmcyInformationDeitles> ChangePharmcyName(string PharmcyId, string PharmcyName);

        Task<bool> ChangePharmcyPassword(string PharmcyId, string password, string newPassword);
    }
}