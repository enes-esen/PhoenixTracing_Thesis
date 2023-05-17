using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Operation_jobs_breaks
    {
        public int break_id { get; set; }
        public int job_id { get; set; }
        public DateTime break_start { get; set; }
        public DateTime break_end { get; set; }
        public DateTime total_break_time { get; set; }
    }
}