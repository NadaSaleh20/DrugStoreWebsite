using DataBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
  public class CurrentOrderAdmin
    {
        public int DrugID { get; set; }

        public string DrugName { get; set; }

        public int Quantity { get; set; }

        public int pricePerUnit { get; set; }

        public float TotalPriceRow { get; set; }

        public float TotalPrice { get; set; }

        public string DateString { get; set; }

        public string pharmcyName { get; set; }

        public int FInalOrderId { get; set; }

        public int OrderId { get; set; }

        public OrderStatus orderStatus { get; set; }
    }
}
