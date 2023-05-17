using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Report
    {
        public int job_id { get; set; }
        public int request_id { get; set; }
        public string per_job_name { get; set; }
        public string per_job_detail { get; set; }        
        public int job_technician_id { get; set; }
        public string job_technician_name { get; set; }
        public System.DateTime job_work_start { get; set; }
        public System.DateTime job_work_end { get; set; }
        public System.TimeSpan job_total_work_time { get; set; }
        public int job_status_id { get; set; }
        public string job_status_name { get; set; }
        public int job_address_id { get; set; }
        public string address_name { get; set; }
        public int step_id { get; set; }
        public string step_code { get; set; }
        public int step_job_id { get; set; }
        public int step_line { get; set; }
        public string step { get; set; }
        public int done { get; set; }
    }
}