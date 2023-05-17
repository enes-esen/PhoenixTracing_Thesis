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
    public class OperationRequestListController : Controller
    {
        [BasicAuth]
        public ActionResult Index()
        {
            return View();
        }

        //Operasyonel iş taleplerini vt'den getirir.
        [BasicAuth]
        public ActionResult OperationRList()
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.Operation_request> content = null;

            string query = @"SELECT op.*
                                   ,opStatus.name AS status
                                   ,opType.name AS request_name
                                   ,ad.name AS address_name
                                   ,ad.phone AS address_phone
                             FROM operation_requests op
                                   ,addresses ad
                                   ,operation_request_status opStatus
                                   ,operation_request_types opType
                             WHERE op.address_id = ad.address_id
                                   AND opStatus.status_id = op.status_id
                                   AND op.request_type_id = opType.type_id
                                   AND ad.user_id = " + user_id + "";
            try
            {
                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Operation_request>();
                List<int> jobID = new List<int>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Operation_request>((Func<DataRow, Phoenix_Tracing.Models.Operation_request>)(Lot => new Phoenix_Tracing.Models.Operation_request()
                    {
                        request_id = Lot.Field<int>("request_id"),

                        request_type_id = Lot.Field<int>("request_type_id"),
                        request_name = Lot.Field<string>("request_name"),

                        name = Lot.Field<string>("name"),
                        detail = Lot.Field<string>("detail"),
                        date = Lot.Field<MySqlDateTime>("date"),

                        status_id = Lot.Field<int>("status_id"),
                        status = Lot.Field<string>("status"),

                        address_id = Lot.Field<int>("address_id"),
                        address_name = Lot.Field<string>("address_name"),
                        address_phone = Lot.Field<string>("address_phone"),

                        active = Lot.Field<int>("active"),

                    })).ToList<Phoenix_Tracing.Models.Operation_request>();

                    //foreach (var id in content)
                    //{                        

                    //}
                }

                return PartialView("OperationRList", content);

            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        //Raporu alınacak operasyonel işin sorgu id'sini getirir.
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
        [BasicAuth]
        public void BuildPDF(int periodic_request_id)
        {
            string session_name = Session["UserName"] + " " + Session["UserSurname"];
                        
            int request_id = Convert.ToInt32(periodic_request_id);
            Phoenix_Tracing.Report.CreateReport reportCreator = new Phoenix_Tracing.Report.CreateReport(request_id, Report.CreateReport.ReportType.Operationel);
            reportCreator.Creator(session_name);            
        }
        #endregion
    }
}