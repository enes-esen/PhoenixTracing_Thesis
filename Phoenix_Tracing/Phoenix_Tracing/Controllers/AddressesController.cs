using MySql.Data.MySqlClient;
using Phoenix_Tracing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phoenix_Tracing.Controllers
{
    public class AddressesController : Controller
    {
        [BasicAuth]
        public ActionResult Index()
        {
            return View();
        }

        //Girilen adres bilgilerini veritabanına gönderir. 
        [BasicAuth, HttpPost]
        public JsonResult SendAddressData(string Name, string City, string Phone, string Address)
        {
            int id = Convert.ToInt32(Session["UserID"]);
            try
            {
                MySqlParameter[] parameters = null;
                int address_id;
                int user_id = id;
                string name = Name;
                string city = City;
                string address = Address;
                string phone = Phone;
                int active = 1;

                string query = @"INSERT INTO addresses(user_id
                                                        ,name
                                                        ,city
                                                        ,address
                                                        ,phone
                                                        ,active)
                                    VALUES(@user_id
                                            ,@name
                                            ,@city
                                            ,@address
                                            ,@phone
                                            ,@active)";

                parameters = new MySqlParameter[6];
                parameters[0] = new MySqlParameter("@user_id", user_id);
                parameters[1] = new MySqlParameter("@name", name);
                parameters[2] = new MySqlParameter("@city", city);
                parameters[3] = new MySqlParameter("@address", address);
                parameters[4] = new MySqlParameter("@phone", phone);
                parameters[5] = new MySqlParameter("@active", active);

                var rdr = MySqlHelper.ExecuteNonQuery(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query, parameters);

                return Json(rdr, JsonRequestBehavior.AllowGet);
            }
            catch (MySqlException ex)
            {
                Result rst = new Result();
                rst.Message = ex.Message;
                return Json(rst, JsonRequestBehavior.AllowGet);
            }
        }

        //Kullanıcıya ait adresi kontrol edet.
        [BasicAuth]
        public int AddressREF(int id)
        {
            Result rst = new Result();
            rst.Ref = 0;
            try
            {
                string query = @"SELECT COUNT(ad.address_id) AS REF
                                FROM addresses ad
                                WHERE ad.user_id = " + id + " ";

                MySqlDataReader rdr = MySqlHelper.ExecuteReader(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                if (rdr.Read())
                {
                    rst.Ref = Convert.ToInt32(rdr["REF"]);
                }
                return rst.Ref;
            }
            catch (MySqlException ex)
            {
                rst = new Result();
                rst.Message = ex.Message;
                rst.Ref = 0;
                return rst.Ref;
            }
        }
    }
}