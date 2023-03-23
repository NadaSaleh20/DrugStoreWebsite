using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class AddPharmcyModel
    {
        [Required]
        public int AccountNum { get; set; }
        [Required]
        public string PharmcyName { get; set; }
     
        public string Password { get; set; }
    }
}
