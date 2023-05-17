using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Operation_jobs
    {
        public int job_id { get; set; }
        public int request_id { get; set; }
        public int technician_id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public int priority { get; set; }
        public int address_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime total_work_time { get; set; }
        public int status_id { get; set; }
        public int active { get; set; }
    }
}