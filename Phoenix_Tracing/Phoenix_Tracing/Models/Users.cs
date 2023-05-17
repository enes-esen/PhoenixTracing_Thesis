using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Users
    {
        public int user_id { get; set; }
        public int user_type_id { get; set; }
        public string user_type_name { get; set; }
        public string mail { get; set; }
        public string password { get; set; }    
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public int active { get; set; }
        public int REF { get; set; }
    }
}