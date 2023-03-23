using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Data
{
    public class PharmcyInfo : IdentityUser
    {
        public int AccountNum { get; set; }

        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
        public List<FinalOrder> FinalOrders { get; set; }

    }
}
