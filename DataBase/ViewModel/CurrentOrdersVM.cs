using DataBase.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.ViewModel
{
    public class CurrentOrdersVM
    {
        public int Count { get; set; }
        public int FilteredCount { get; set; }
        public List<GetCurrentOrderDtos> Orders { get; set; }
    }
}
