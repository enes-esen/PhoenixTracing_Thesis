using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    public class Operation_request
    {
        public int request_id { get; set; }
        public int request_type_id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public MySql.Data.Types.MySqlDateTime date { get; set; }
        public int status_id { get; set; }
        public int address_id { get; set; }
        public int active { get; set; }


        #region SonradanEklenenler
        public string request_name { get; set; }
        public string status { get; set; }
        public string address_name { get; set; }
        public string address_phone { get; set; }
        #endregion
    }
}