using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class ChangPassword
    {

        public string PharmcyId { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
