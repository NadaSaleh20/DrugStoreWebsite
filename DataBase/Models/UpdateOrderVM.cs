using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class UpdateOrderVM
    {
        public int drugId { get; set; }
        public int orderId { get; set; }
        public int QuaNew { get; set; }
        public string pharmcyName { get; set; }

        public int finalOrderId { get; set; }
        public string DrugName { get; set; }
    }
}
