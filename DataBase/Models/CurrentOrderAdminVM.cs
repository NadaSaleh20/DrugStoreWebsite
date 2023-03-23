using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
   public class CurrentOrderAdminVM
    {
        public int Count { get; set; }
        public int FilteredCount { get; set; }
        public List<CurrentOrderAdmin> Orders { get; set; }
    }
}
