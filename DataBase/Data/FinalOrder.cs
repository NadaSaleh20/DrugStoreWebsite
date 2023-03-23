using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Data
{
    public class FinalOrder
    {
        public int Id { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }

        public List<OrderItem> orderItems { get; set; }

        public PharmcyInfo pharmcyInfo { get; set; }

    }
}
