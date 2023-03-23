using DataBase.Data;
using DrugStoreApplication__MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Repostry
{
    public class AdminMangment : IAdminMangment
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<PharmcyInfo> _userManager;

        public AdminMangment(ApplicationDBContext context , UserManager<PharmcyInfo> userManager)
        {
             _context = context;
           _userManager = userManager;
        }

        public async Task ImportDrugsToDataBase(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var list = new List<Drug>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var packet = new ExcelPackage(stream))
                {
                    ExcelWorksheet excelWorksheet = packet.Workbook.Worksheets[0];
                    var rowCount = excelWorksheet.Dimension.Rows;
                    for (int row = 2; row < rowCount; row++)
                    {
                        list.Add(new Drug
                        {
                            DrugName = excelWorksheet.Cells[row, 2].Value?.ToString().Trim(),
                            QuantityStorage = (int)Convert.ToSingle(excelWorksheet.Cells[row, 3]?.Value),
                            Price = (int)Convert.ToSingle(excelWorksheet.Cells[row, 4]?.Value),

                        });
                    }
                }
            }

            _context.Drug.AddRange(list);
            await _context.SaveChangesAsync();
        }


        public async Task ImportPharmciesToDataBase(IFormFile file)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var list = new List<Customer>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var packet = new ExcelPackage(stream))
                {
                    ExcelWorksheet excelWorksheet = packet.Workbook.Worksheets[0];
                    var rowCount = excelWorksheet.Dimension.Rows;
                    for (int row = 2; row < rowCount; row++)
                    {
                        list.Add(new Customer
                        {
                            PharmcyName = excelWorksheet.Cells[row, 2].Value?.ToString().Trim(),
                            AccountNumber = (int)Convert.ToSingle(excelWorksheet.Cells[row, 1]?.Value)

                        });
                    }
                }
            }
            foreach (var pharmcy in list)
            {
                var pharmcyDetiles = new PharmcyInfo
                {
                    AccountNum = pharmcy.AccountNumber,
                    UserName = pharmcy.PharmcyName
                };

                var result = await _userManager.CreateAsync(pharmcyDetiles, "Ap@123456");
                await _userManager.AddToRoleAsync(pharmcyDetiles, "User");
            }  
        }

    

    }
}
