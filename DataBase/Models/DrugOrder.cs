﻿using DataBase.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
   public  class DrugOrder
    {
        public int DrugID { get; set; }

        public string DrugName { get; set; }

        public int Quantity { get; set; }

        public bool IsAvialable { get; set; }

        public int pricePerUnit { get; set; }

        public float TotalPriceRow { get; set; }

        public OrderStatus Status { get; set; }

        public string DateString { get; set; } 
        public float TotalPrice { get; set; }
    }
}
