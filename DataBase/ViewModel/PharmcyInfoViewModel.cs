using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.ViewModel
{
   public class PharmcyInfoViewModel 
    {
        public string Id { get; set; }

        public string PharmcyName { get; set; }
        public int AccountNum { get; set; }

        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
    }
}
