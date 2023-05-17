using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Carrier_jobs
    {
        public int job_id { get; set; }
        public int technician_id { get; set; }
        public int carrier_id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public int address_id { get; set; }
        public int priority { get; set; }
        public int status_id { get; set; }
        public int active { get; set; }
    }
}