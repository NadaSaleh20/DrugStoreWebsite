using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Data
{
    public class Drug
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int Price { get; set; }
        public int QuantityStorage { get; set; }

        public List<OrderItem> orderItems { get; set; }
    }
}
