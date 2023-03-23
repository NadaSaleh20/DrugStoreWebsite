using DataBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Dtos
{
    public class GetCurrentOrderDtos
    {
        public int DrugID { get; set; }

        public string DrugName { get; set; }

        public int Quantity { get; set; }

        public bool IsAvialable { get; set; }

        public int pricePerUnit { get; set; }

        public float TotalPriceRow { get; set; }

        public OrderStatus Status { get; set; }

        public float TotalPrice { get; set; }

        public string DateString { get; set; }

        public int OrderId { get; set; }

        public int finalOrderId { get; set; }
    }
}
