using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Job_Step
    {
        public int step_id { get; set; }
        public string step { get; set; }
        public int done { get; set; }
    }
}