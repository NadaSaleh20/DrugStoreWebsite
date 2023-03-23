using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
   public class SaveRowsOrder
    {
        public string PharmcyID { get; set; }

        public DrugInfoObjectRows rows { get; set; }
    }
}
