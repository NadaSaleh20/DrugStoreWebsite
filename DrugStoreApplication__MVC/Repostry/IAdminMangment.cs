using DataBase.Data;
using DrugStoreApplication__MVC.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Repostry
{
    public interface IAdminMangment
    {
         Task ImportDrugsToDataBase(IFormFile file);
        Task ImportPharmciesToDataBase(IFormFile file);
    }
}