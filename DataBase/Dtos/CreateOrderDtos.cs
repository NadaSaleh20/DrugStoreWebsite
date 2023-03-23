using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Dtos
{
    public class CreateOrderDtos
    {
        public int DrugID { get; set; }

        public string DrugName { get; set; }

        public int Quantity { get; set; }

        public bool IsAvialable { get; set; }

        public int pricePerUnit { get; set; }

        public float TotalPriceRow { get; set; }
    }
}
