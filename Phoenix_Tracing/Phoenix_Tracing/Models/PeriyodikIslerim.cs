using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class PeriyodikIslerim
    {
        public int job_id { get; set; }
        public int request_id { get; set; }
        public string per_job_name { get; set; }
        public string per_job_detail { get; set; }
        public int status_id { get; set; }
        public string per_job_status_name { get; set; }
        public int address_id { get; set; }
        public string address_name { get; set; }
        public string address_phone { get; set; }
        public int per_request_period { get; set; }
        public MySql.Data.Types.MySqlDateTime per_request_date { get; set; }
        public MySql.Data.Types.MySqlDateTime per_request_afterDate { get; set; }
        public int request_type_id { get; set; }
        public string per_request_type_name { get; set; }
    }
}