using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Contacts
    {
        public int contact_id { get; set; }
        public string full_name { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string message_title { get; set; }
        public string message { get; set; }
        public int active { get; set; }
    }
}