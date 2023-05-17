using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Operation_steps
    {
        public int step_id { get; set; }
        public int request_id { get; set; }
        public int step_line { get; set; }
        public string step { get; set; }
        public int done { get; set; }
    }
}