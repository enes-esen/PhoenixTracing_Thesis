using MySql.Data.MySqlClient;
using MySql.Data.Types;
using Phoenix_Tracing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phoenix_Tracing.Controllers
{
    public class HelpRequestListController : Controller
    {
        [BasicAuth]
        public ActionResult Index()
        {
            return View();
        }
        
        //Destek taleplerini vt'den getirir.
        [BasicAuth]
        public ActionResult HelpRList()
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.Help_request> content = null;

            string query = @"SELECT h.*, ht.name AS statusName
                            FROM help_requests h, help_request_status ht
                            WHERE h.status_id = ht.status_id
                            AND user_id = " + user_id +"";
            try
            {
                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Help_request>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Help_request>((Func<DataRow, Phoenix_Tracing.Models.Help_request>)(Lot => new Phoenix_Tracing.Models.Help_request()
                    {
                        request_id = Lot.Field<int>("request_id"),
                        user_id = Lot.Field<int>("user_id"),
                        destination_users = Lot.Field<int>("destination_users"),
                        name = Lot.Field<string>("name"),
                        detail = Lot.Field<string>("detail"),
                        date = Lot.Field<MySqlDateTime>("date"),
                        status_id = Lot.Field<int>("status_id"),
                        active = Lot.Field<int>("active"),
                        statusName = Lot.Field<string>("statusName"),

                    })).ToList<Phoenix_Tracing.Models.Help_request>();
                }
                return PartialView("HelpRList", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }            
        }
    }
}