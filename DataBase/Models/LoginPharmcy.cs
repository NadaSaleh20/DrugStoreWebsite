
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class LoginPharmcy
    {
        [Required]
        public int AccountNum { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
