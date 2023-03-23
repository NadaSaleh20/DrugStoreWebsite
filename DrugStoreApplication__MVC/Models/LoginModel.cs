using DataBase.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Models
{
    public class LoginModel
    {
        [Required]
        public int PharmcyCode { get; set; }

        [Required , DataType(DataType.Password)]
        public string PharmcyPassword { get; set; }

        public bool RembmerMe { get; set; }

        
    }
}
