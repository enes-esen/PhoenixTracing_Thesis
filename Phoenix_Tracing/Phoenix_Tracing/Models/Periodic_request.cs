using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Periodic_request
    {
        public int request_id { get; set; }
        public int request_type_id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public MySql.Data.Types.MySqlDateTime date { get; set; }
        public int status_id { get; set; }
        public int period { get; set; }
        public int address_id { get; set; }
        public int active { get; set; }

        #region Sonradan eklenenler
        public string typeName { get; set; }
        public string statusName { get; set; }
        public string addressName { get; set; }
        public string addressPhone { get; set; }
        public MySql.Data.Types.MySqlDateTime afterDate { get;set; }
        #endregion

        #region Job Durumları
        public int job_status_id { get; set; }
        public string job_status_name { get; set; }
        #endregion
    }
}