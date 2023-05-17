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
    //Periyodik iş talebinde bulunma
    public class PeriodicRequestsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        // Periyodik iş taleplerini vt'ye  gönderir
        [BasicAuth, HttpPost]
        public JsonResult SendPeriodicData(string Name, int Request_type_id, int Address_id, int Period, string Detail)
        {
            MySqlParameter[] parameters = null;
            int request_type_id = Request_type_id;
            string name = Name;
            string detail = Detail;
            int status_id = 1;  
            int period = Period;
            int address_id = Address_id;
            int active = 0;

            try
            {
                string query = @"INSERT INTO periodic_requests(request_type_id
                                                                ,name
                                                                ,detail
                                                                ,date
                                                                ,status_id
                                                                ,period
                                                                ,address_id
                                                                ,active)
                                VALUES(@request_type_id
                                       ,@name 
                                       ,@detail 
                                       ,CURDATE() 
                                       ,@status_id
                                       ,@period
                                       ,@address_id
                                       ,@active)";

                parameters = new MySqlParameter[7];
                parameters[0] = new MySqlParameter("request_type_id", request_type_id);
                parameters[1] = new MySqlParameter("name", name);
                parameters[2] = new MySqlParameter("detail", detail);
                parameters[3] = new MySqlParameter("status_id", status_id);
                parameters[4] = new MySqlParameter("period", period);
                parameters[5] = new MySqlParameter("address_id", address_id);
                parameters[6] = new MySqlParameter("active", active);

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

        #region CheckPeriodic
        // Kullanıcıya ait adresleri vt'den getirir
        [BasicAuth]
        public ActionResult CheckAddressData()
        {
            Phoenix_Tracing.Controllers.AddressesController controllerAddress = null;
            try
            {
                int id = Convert.ToInt32(Session["UserID"]);
                controllerAddress = new AddressesController();
                var address_REF = controllerAddress.AddressREF(id);

                List<Phoenix_Tracing.Models.Addresses> content = null;

                if (address_REF >= 0)
                {
                    string query = @"SELECT ad.address_id, ad.name, ad.active
                                    FROM addresses ad
                                    WHERE ad.active = 1 AND ad.user_id = " + id + "";

                    DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                    content = new List<Addresses>();
                    if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                    {
                        content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Addresses>((Func<DataRow, Phoenix_Tracing.Models.Addresses>)(Lot => new Phoenix_Tracing.Models.Addresses()
                        {
                            address_id = Lot.Field<int>("address_id"),
                            name = Lot.Field<string>("name"),
                            active = Lot.Field<int>("active")
                        })).ToList<Phoenix_Tracing.Models.Addresses>();

                        return PartialView("CheckAddressData", content);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Phoenix_Tracing.Models.Result rst = new Phoenix_Tracing.Models.Result();
                rst.Message = ex.Message;
                rst.Ref = 0;
                return PartialView("CheckAddressData", rst.Message);
            }
            return View();
        }

        //Operasyonel iş tiplerini vt'den getirir.
        [BasicAuth]
        public ActionResult CheckPeriodicType()
        {
            List<Phoenix_Tracing.Models.Periodic_request_types> content = null;
            try
            {
                string query = @"SELECT *
                                FROM periodic_request_types pert";

                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Periodic_request_types>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Periodic_request_types>((Func<DataRow, Phoenix_Tracing.Models.Periodic_request_types>)(Lot => new Phoenix_Tracing.Models.Periodic_request_types()
                    {
                        type_id = Lot.Field<int>("type_id"),
                        name = Lot.Field<string>("name")
                    })).ToList<Phoenix_Tracing.Models.Periodic_request_types>();
                }
                return PartialView("CheckPeriodicType", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
        #endregion
    }
}