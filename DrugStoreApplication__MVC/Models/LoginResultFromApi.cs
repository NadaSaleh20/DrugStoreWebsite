using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugStoreApplication__MVC.Models
{
    public class LoginResultFromApi
    {
        public string Token { get; set; }
        public List<string> rules { get; set; }
    }
}
