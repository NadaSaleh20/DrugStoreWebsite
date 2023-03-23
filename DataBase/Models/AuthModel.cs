using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public List<string> Rules { get; set; }
        public DateTime ExpirseOn { get; set; }
        public bool IsAuthntecated { get; set; }
    }
}
