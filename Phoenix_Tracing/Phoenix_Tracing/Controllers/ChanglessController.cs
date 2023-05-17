using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phoenix_Tracing.Controllers
{
    //Karşılama Sayfası Backend kısmı
    public class ChanglessController : Controller
    {
        public ActionResult FirstMeeting()
        {
            return View();
        }

        //Karşılama sayfasından iletişim kutusundan gelen verileri vt'ye gönderir.
        [HttpPost]
        public JsonResult SendDataContacts(string full_name, string mail, string phone, string message_title, string message)
        {
            try
            {
                Phoenix_Tracing.Models.Contacts item = null;
                MySqlParameter[] parameters = null;
                int active = 1;

                string query = @"INSERT INTO contacts(full_name
                                                    ,mail
                                                    ,phone
                                                    ,message_title
                                                    ,message
                                                    ,active)
                                VALUES(@full_name
                                        ,@mail
                                        ,@phone
                                        ,@message_title
                                        ,@message
                                        ,1)";

                item = new Models.Contacts();
                item.full_name = full_name;
                item.mail = mail;
                item.phone = phone;
                item.message_title = message_title;
                item.message = message;
                item.active = active;

                parameters = new MySqlParameter[5];
                parameters[0] = new MySqlParameter("@full_name", item.full_name);
                parameters[1] = new MySqlParameter("@mail", item.mail);
                parameters[2] = new MySqlParameter("@phone", item.phone);
                parameters[3] = new MySqlParameter("@message_title", item.message_title);
                parameters[4] = new MySqlParameter("@message", item.message);

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