using DrugStoreApplication__MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
   public class GetPharmcyiesPaging
    {
        public int Count { get; set; }

        public int FilterdCount { get; set; }

        public List<Customer> Customers { get; set; }
    }
}
