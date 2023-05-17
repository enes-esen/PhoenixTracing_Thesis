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
    //Kullanıcı düzenleme sayfası
    public class xProfileEditController : Controller
    {
        [BasicAuth]
        public ActionResult Index()
        {
            return View();
        }

        // Kullanıcı bilgilerini düzenleme sayfasını vt'ye getirir.
        [BaasicAuth]
        public ActionResult GetUserList()
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.Users> content = null;
            DataSet dt = null;
            string query = @"SELECT u.user_id
                            		,u.mail
                            		,u.name
                            		,u.surname
                            		,u.phone
                            		,u.active
                            FROM users u
                            WHERE user_id = " + user_id + "";

            try
            {
                dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Users>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Users>((Func<DataRow, Phoenix_Tracing.Models.Users>)(Lot => new Phoenix_Tracing.Models.Users()
                    {
                        user_id = Lot.Field<int>("user_id"),
                        mail = Lot.Field<string>("mail"),
                        name = Lot.Field<string>("name"),
                        surname = Lot.Field<string>("surname"),
                        phone = Lot.Field<string>("phone"),
                        active = Lot.Field<int>("active"),
                    })).ToList<Phoenix_Tracing.Models.Users>();
                }
                return PartialView("GetUserList", content);

            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
        
        // Kullanıcıya ait adresleri vt'den getirir.
        [BaasicAuth]
        public ActionResult GetUserAddressNames()
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.Addresses> content = null;
            DataSet dt = null;
            string query = @"SELECT ad.address_id
		                            ,ad.user_id
		                            ,ad.name
                            FROM addresses ad
                            WHERE ad.active = 1
                                AND ad.user_id = " + user_id + "";

            try
            {
                dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Addresses>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Addresses>((Func<DataRow, Phoenix_Tracing.Models.Addresses>)(Lot => new Phoenix_Tracing.Models.Addresses()
                    {
                        address_id = Lot.Field<int>("address_id"),
                        user_id = Lot.Field<int>("user_id"),
                        name = Lot.Field<string>("name"),

                    })).ToList<Phoenix_Tracing.Models.Addresses>();
                }
                return PartialView("GetUserAddressNames", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        // Sayfada seçilen adresin bilgilerini vt'den getirir.
        [BaasicAuth]
        public JsonResult GetUserAddressData(string addressID)
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.Addresses> content = null;
            DataSet dt = null;
            string query = @"SELECT adr.address_id
                                    ,adr.name
                            		,adr.city
                            		,adr.phone
                            		,adr.address
                            FROM addresses adr
                            WHERE adr.address_id = " + addressID + " AND adr.user_id = " + user_id + "";

            try
            {
                dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Addresses>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Addresses>((Func<DataRow, Phoenix_Tracing.Models.Addresses>)(Lot => new Phoenix_Tracing.Models.Addresses()
                    {
                        address_id = Lot.Field<int>("address_id"),
                        name = Lot.Field<string>("name"),
                        city = Lot.Field<string>("city"),
                        phone = Lot.Field<string>("phone"),
                        address = Lot.Field<string>("address"),
                    })).ToList<Phoenix_Tracing.Models.Addresses>();
                }                
                return Json(content, JsonRequestBehavior.AllowGet);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        // Kullanıcı bilgi güncellemeleri
        [BasicAuth, HttpPost]
        public JsonResult UserInformationEdit(string name, string surname, string phone, string mail)
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            MySqlParameter[] parameters = null;

            string query = @"UPDATE users u 
                            SET u.name = @name 
                            	,u.surname = @surname 
                            	,u.phone = @phone
                            	,u.mail = @mail
                            WHERE u.user_id = "+user_id+"";

            try
            {
                parameters = new MySqlParameter[4];
                parameters[0] = new MySqlParameter("@name", name);
                parameters[1] = new MySqlParameter("@surname", surname);
                parameters[2] = new MySqlParameter("@phone", phone);
                parameters[3] = new MySqlParameter("@mail", mail);

                var rdr = MySqlHelper.ExecuteNonQuery(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query, parameters);

                return Json(rdr, JsonRequestBehavior.AllowGet);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        //Adres Bilgisini düzenleme
        [BasicAuth, HttpPost]
        public JsonResult AddressInformationEdit(int addressID, string phone, string city, string address)
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            MySqlParameter[] parameters = null;
            string query = @"UPDATE addresses a 
                             SET a.phone = @phone  
                             		,a.city = @city 
                             		,a.address = @address
                             WHERE a.user_id = "+user_id+" AND a.address_id = "+addressID+"";

            try
            {
                parameters = new MySqlParameter[3];
                parameters[0] = new MySqlParameter("@phone", phone);
                parameters[1] = new MySqlParameter("@city", city);
                parameters[2] = new MySqlParameter("@address", address);
                //parameters[3] = new MySqlParameter("@mail", mail);

                var rdr = MySqlHelper.ExecuteNonQuery(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query, parameters);

                return Json(rdr, JsonRequestBehavior.AllowGet);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
    }
}