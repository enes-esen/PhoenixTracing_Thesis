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
    // Kullanıcı bilgilerini içeren sayfa
    public class ProfileController : Controller
    {
        [BasicAuth]
        public ActionResult Index()
        {
            return View();
        }
        
        // Kullanıcı bilgisini vt'den getirir.
        [BasicAuth]
        public ActionResult ListProfile()
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
                            WHERE user_id = "+user_id+"";

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
            return PartialView("ListProfile", content);

            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
        
        // Kullanıcıya ait adresleri vt'den getirir.
        [BasicAuth]
        public ActionResult ListAddress()
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.Addresses> content = null;
            DataSet dt = null;
            string query = @"SELECT ad.user_id
                            		,ad.name
                            		,ad.phone
                            		,ad.address
                            FROM addresses ad
                            WHERE ad.active = 1 AND ad.user_id = " + user_id + "";

            try
            {
                dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Addresses>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Addresses>((Func<DataRow, Phoenix_Tracing.Models.Addresses>)(Lot => new Phoenix_Tracing.Models.Addresses()
                    {
                        user_id = Lot.Field<int>("user_id"),
                        name = Lot.Field<string>("name"),
                        phone = Lot.Field<string>("phone"),
                        address = Lot.Field<string>("address"),
                    })).ToList<Phoenix_Tracing.Models.Addresses>();
                }
                return PartialView("ListAddress", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
    }
}