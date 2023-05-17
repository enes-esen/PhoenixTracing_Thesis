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
    public class PeriodicRequestListController : Controller
    {
        // GET: PeriodicRequestList
        [BasicAuth]
        public ActionResult Index()
        {
            return View();
        }

        //Periyodik iş taleplerini vt'den getirir.
        [BasicAuth]
        public ActionResult PeriodicRList()
        {
            //AddMonths(ay) : belirtilen ay kadar mevcut tarihe ay eklemesi yapar.
            int user_id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.Periodic_request> content = null;

            string query = @"SELECT DISTINCT perR.request_id
                            	,perR.name AS request_name
                            	,perR.detail AS request_detail
                            	,perR.request_type_id
                            	,perRT.name AS request_type_name
                            	,perR.status_id AS request_status_id
                            	,perRS.name AS request_status_name
                            	,perR.period AS request_period
                            	,perR.date AS request_date
                            	,ADDDATE(perR.date, INTERVAL perR.period MONTH) AS request_afterDate
                            	,perR.address_id AS request_address_id
                            	,a.name AS address_name
                            	,a.phone AS address_phone
                            FROM periodic_requests perR
                            		,periodic_request_status perRS
                            		,periodic_request_types perRT
                            		,users u
                            		,addresses a
                            WHERE perR.address_id = a.address_id
                            	AND perR.request_type_id = perRT.type_id
                            	AND perR.status_id = perRS.status_id
                            	AND a.user_id = u.user_id
                            	AND u.user_id = "+user_id+"";
            try
            {
                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Periodic_request>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Periodic_request>((Func<DataRow, Phoenix_Tracing.Models.Periodic_request>)(Lot => new Phoenix_Tracing.Models.Periodic_request()
                    {
                        request_id = Lot.Field<int>("request_id"),
                        name = Lot.Field<string>("request_name"),
                        detail = Lot.Field<string>("request_detail"),

                        request_type_id = Lot.Field<int>("request_type_id"),
                        typeName = Lot.Field<string>("request_type_name"),

                        status_id = Lot.Field<int>("request_status_id"),
                        statusName = Lot.Field<string>("request_status_name"),

                        period = Lot.Field<int>("request_period"),
                        
                        date = Lot.Field<MySqlDateTime>("request_date"),
                        afterDate = Lot.Field<MySqlDateTime>("request_afterDate"),

                        address_id = Lot.Field<int>("request_address_id"),
                        addressName = Lot.Field<string>("address_name"),
                        addressPhone = Lot.Field<string>("address_phone"),
                    })).ToList<Phoenix_Tracing.Models.Periodic_request>();
                }
                return PartialView("PeriodicRList", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        //Raporu alınacak periyodik işin sorgu id'sini getirir.
        [BasicAuth]
        public JsonResult RequestID(int request_id)
        {
            int job_id_ = request_id;
            BuildPDF(request_id);
            string message = "Rapor İndirilenler Klasöründe.";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #region Create PDF
        //Raporu oluşturulacak fonksiyona sorgu id'yi gönderir.
        [BasicAuth, HttpPost]
        public ActionResult BuildPDF(int periodic_request_id)
        {
            string session_name = Session["UserName"] + " " + Session["UserSurname"];

            int request_id = Convert.ToInt32(periodic_request_id);
            Phoenix_Tracing.Report.CreateReport reportCreator = new Phoenix_Tracing.Report.CreateReport(request_id, Report.CreateReport.ReportType.Periodic);
            reportCreator.Creator(session_name);

            return View();
        }
        #endregion
    }
}