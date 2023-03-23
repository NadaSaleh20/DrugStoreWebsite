using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    class currentOrders
    {
        public int Count { get; set; }
        public int FilteredCount { get; set; }
        public List<DrugOrder> Orders { get; set; }

    }
}
