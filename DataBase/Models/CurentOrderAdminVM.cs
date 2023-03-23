using System;

namespace DataBase.Models
{
    public class CurentOrderAdminVM
    {
     
        public int? drugId { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string pharmcyId { get; set; }
        //public string minDate { get; set; }
        //public string maxDate { get; set; }
        public DateTime? minDate { get; set; }
        public DateTime? maxDate { get; set; }

    }
  
}
