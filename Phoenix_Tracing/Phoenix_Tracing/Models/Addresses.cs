using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Addresses
    {
        public int address_id { get; set; }
        public int user_id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int active { get; set; }
        public int REF { get; set; }
    }
}