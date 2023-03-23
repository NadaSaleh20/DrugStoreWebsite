using DataBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Data
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int QunatityOrder { get; set; }
        public OrderStatus Status { get; set; }
        public Drug Drug { get; set; }

        public FinalOrder FinalOrder { get; set; }


    }
}
