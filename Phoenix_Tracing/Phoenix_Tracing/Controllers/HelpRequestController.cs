using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Phoenix_Tracing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phoenix_Tracing.Controllers
{
    // Destek talebinde bulunma
    public class HelpRequestController : Controller
    {
        [BasicAuth]
        public ActionResult Index()
        {
            return View();
        }

        // Destek taleplerini vt'ye gönderir.
        [HttpPost, BasicAuth]
        public JsonResult SendDataHelpRequest(string name, string detail)
        {
            try
            {
                Phoenix_Tracing.Models.Help_request item = null;
                MySqlParameter[] parameters = null;

                int user_id = Convert.ToInt32(Session["UserID"]);
                
                int destination_users = 3;
                int status_id = 1;
                int active = 0;

                string query = @"INSERT INTO 
                                help_requests(user_id
                                				,destination_users
                                				,name
                                				,detail
                                                ,date
                                				,status_id
                                				,active
                                )
                                VALUES(@user_id
                                        ,@destination_users
                                        ,@name
                                        ,@detail
                                        ,CURRENT_TIMESTAMP()
                                        ,@status_id
                                        ,@active)";


                item = new Models.Help_request();
                item.user_id = user_id;
                item.destination_users = destination_users;
                item.name = name;
                item.detail = detail;
                item.status_id = status_id;
                item.active = active;

                parameters = new MySqlParameter[6];
                parameters[0] = new MySqlParameter("@user_id",item.user_id);
                parameters[1] = new MySqlParameter("@destination_users", item.destination_users);
                parameters[2] = new MySqlParameter("@name", item.name);
                parameters[3] = new MySqlParameter("@detail", item.detail);
                parameters[4] = new MySqlParameter("@status_id", item.status_id);
                parameters[5] = new MySqlParameter("@active", item.active);

                var rdr = MySqlHelper.ExecuteNonQuery(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query, parameters);

                return Json(rdr, JsonRequestBehavior.AllowGet);
            }
            catch (MySqlException ex)
            {
                Phoenix_Tracing.Models.Result rst = new Models.Result();
                rst.Message = ex.Message;

                return Json(rst, JsonRequestBehavior.AllowGet);                
            }            
        }
    }
}