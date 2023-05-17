using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Help_request
    {
        public int request_id { get; set; }
        public int user_id { get; set; }
        public int destination_users { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public MySql.Data.Types.MySqlDateTime date { get; set; }
        public int status_id { get; set; }
        public int active { get; set; }
        public string statusName { get; set; }
    }
}