using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Phoenix_Tracing.Controllers;
using Phoenix_Tracing.Models;
using System.Web.Security;
using System.Data;
using MySql.Data.Types;

namespace Phoenix_Tracing.Controllers
{
    // Dashboard Sayfası
    public class HomeController : Controller
    {
        public JsonResult Dene(string data)
        {
            return Json("text-danger", JsonRequestBehavior.AllowGet);
        }

        [BasicAuth]
        public ActionResult Index()
        {            
            return View();
        }

        #region Requests
        // Destek taleplerini vt'den getirir.
        [BasicAuth]
        public ActionResult HelpRequest()
        {
            int id = Convert.ToInt32(Session["UserID"]);

            List<Phoenix_Tracing.Models.Help_request> content = null;
            try
            {
                string query = @"SELECT h.*, ht.name AS statusName
                        FROM help_requests h, help_request_status ht
                        WHERE h.status_id = ht.status_id
                        AND user_id = " + id + "";

                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Help_request>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Help_request>((Func<DataRow, Phoenix_Tracing.Models.Help_request>)(Lot => new Phoenix_Tracing.Models.Help_request()
                    {
                        //Lot.Field<int>("LOGREF"),
                        request_id = Lot.Field<int>("request_id"),
                        user_id = Lot.Field<int>("user_id"),
                        destination_users = Lot.Field<int>("destination_users"),
                        name = Lot.Field<string>("name"),
                        detail = Lot.Field<string>("detail"),
                        date = Lot.Field<MySqlDateTime>("date"),
                        //date = Lot.Field<DateTime>("date"),
                        status_id = Lot.Field<int>("status_id"),
                        active = Lot.Field<int>("active"),
                        statusName = Lot.Field<string>("statusName"),

                    })).ToList<Phoenix_Tracing.Models.Help_request>();
                }
                return PartialView("HelpRequest", content);
            }
            catch(MySqlException ex)
            {
                Result rst = new Result();
                rst.Message = ex.Message;
                return PartialView("PeriodicRequest", rst);
            }
        }
        
        // Operasyonel iş taleplerini vt'den getirir.
        [BasicAuth]
        public ActionResult OperationRequest()
        {
            int id = Convert.ToInt32(Session["UserID"]);

            List<Phoenix_Tracing.Models.Operation_request> content = null;
            try
            {
                string query = @"SELECT op.request_id
                                        ,op.request_type_id
                                        ,opType.name AS request_name
                                        ,op.name
                                        ,op.detail
                                        ,op.status_id
                                        ,opStatus.name AS status
                                        ,op.address_id
                                        ,ad.name AS address_name
                                        ,op.active
                                        ,op.date
                                FROM operation_requests op
                                    ,addresses ad
                                    ,operation_request_status opStatus
                                    ,operation_request_types opType
                                WHERE op.address_id = ad.address_id
                                        AND opStatus.status_id = op.status_id
                                        AND op.request_type_id = opType.type_id
                                        AND ad.user_id = " + id + "";


                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Operation_request>();
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
                        active = Lot.Field<int>("address_id"),

                    })).ToList<Phoenix_Tracing.Models.Operation_request>();                    
                }
                return PartialView("OperationRequest", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        // Periyodik iş taleplerini vt'den getirir.
        [BasicAuth]
        public ActionResult PeriodicRequest()
        {
            int user_id = Convert.ToInt32(Session["UserID"]);

            List<Phoenix_Tracing.Models.Periodic_request> content = null;
            try
            {                
                string query = @"SELECT per.*
                        			,ad.name AS addressName
                        			,ad.phone AS addressPhone
                        			,perT.name AS typeName
                        			,perS.name AS statusName
                                    ,ADDDATE(per.date, INTERVAL per.period MONTH) AS afterDate
                            FROM periodic_requests per
                        		,addresses ad
                        		,periodic_request_types perT
                        		,periodic_request_status perS
                            WHERE per.address_id = ad.address_id 
                        	    AND per.request_type_id = perT.type_id
                        	    AND per.status_id = perS.status_id
                        	    AND per.address_id IN (
                        							SELECT ad.address_id
                        							FROM addresses ad
                        							WHERE ad.user_id = " + user_id + ")";

                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Periodic_request>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.Periodic_request>((Func<DataRow, Phoenix_Tracing.Models.Periodic_request>)(Lot => new Phoenix_Tracing.Models.Periodic_request()
                    {
                        request_id = Lot.Field<int>("request_id"),
                        request_type_id = Lot.Field<int>("request_type_id"),
                        typeName = Lot.Field<string>("typeName"),
                        name = Lot.Field<string>("name"),
                        detail = Lot.Field<string>("detail"),
                        date = Lot.Field<MySqlDateTime>("date"),
                        status_id = Lot.Field<int>("status_id"),
                        statusName = Lot.Field<string>("statusName"),
                        period = Lot.Field<int>("period"),
                        address_id = Lot.Field<int>("address_id"),
                        addressName = Lot.Field<string>("addressName"),
                        active = Lot.Field<int>("active")

                    })).ToList<Phoenix_Tracing.Models.Periodic_request>();
                }
                return PartialView("PeriodicRequest", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
        #endregion

        #region Totals
        // Toplam destek talep sayısını vt'den getirir.
        [BasicAuth]
        public ActionResult HelpTotal()
        {
            int id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.TotalWork.Help_total> content = null;
            try
            {
                string query = @"SELECT COUNT(hReq.request_id) AS 'helpTotal'
                                    FROM help_requests hReq
                                    WHERE hReq.user_id=" + id+"";


                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Phoenix_Tracing.Models.TotalWork.Help_total>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.TotalWork.Help_total>((Func<DataRow, Phoenix_Tracing.Models.TotalWork.Help_total>)(Lot => new Phoenix_Tracing.Models.TotalWork.Help_total()
                    {
                        //Sql sorguda count Int64 biçiminde gelmektedir.
                        helpTotal = Lot.Field<Int64>("helpTotal")

                    })).ToList<Phoenix_Tracing.Models.TotalWork.Help_total>();
                }
                return PartialView("HelpTotal", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        // Toplam operasyonel iş talep sayısını vt'den getirir.
        [BasicAuth]
        public ActionResult OperationTotal()
        {
            int id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.TotalWork.Operation_total> content = null;
            try
            {
                string query = @"SELECT  COUNT(oReq.request_id) AS operationTotal
                                FROM operation_requests oReq
                                WHERE oReq.address_id IN(
                                	SELECT ad.address_id
                                	FROM addresses ad
                                	WHERE ad.user_id = " + id+")";


                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Models.TotalWork.Operation_total>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.TotalWork.Operation_total>((Func<DataRow, Phoenix_Tracing.Models.TotalWork.Operation_total>)(Lot => new Phoenix_Tracing.Models.TotalWork.Operation_total()
                    {
                        operationTotal = Lot.Field<Int64>("operationTotal")

                    })).ToList<Phoenix_Tracing.Models.TotalWork.Operation_total>();
                }
                return PartialView("OperationTotal", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        // Toplam periyodik iş talep sayısını vt'den getirir.
        [BasicAuth]
        public ActionResult PeriodTotal()
        {
            int id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.TotalWork.Periodic_total> content = null;
            try
            {
                string query = @"SELECT COUNT(perReq.request_id) AS periodicTotal
                                FROM periodic_requests perReq
                                WHERE perReq.address_id IN(
                                	SELECT ad.address_id
                                	FROM addresses ad
                                	WHERE ad.user_id = "+id+")";


                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Models.TotalWork.Periodic_total>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.TotalWork.Periodic_total>((Func<DataRow, Phoenix_Tracing.Models.TotalWork.Periodic_total>)(Lot => new Phoenix_Tracing.Models.TotalWork.Periodic_total()
                    {
                        periodicTotal = Lot.Field<Int64>("periodicTotal")

                    })).ToList<Phoenix_Tracing.Models.TotalWork.Periodic_total>();
                }
                return PartialView("PeriodTotal", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
        #endregion

        #region Grafik
        // Grafik için toplam periyodik iş talep sayısını vt'den getirir.
        [BasicAuth]
        public JsonResult GraphicTotal()
        {
            int id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.TotalWork.Total> content = null;
            try
            {
                string query = @"SELECT COUNT(helpR.request_id) AS total
                                FROM help_requests helpR
                                WHERE helpR.user_id = " + id + "" +
                                " UNION" +
                                " SELECT COUNT(opR.request_id) AS operationTotal FROM operation_requests opR WHERE opR.address_id IN(SELECT ad.address_id" +
                                                                                                            " FROM addresses ad WHERE ad.user_id = "+id+")" +
                                " UNION" +
                                " SELECT COUNT(perReq.request_id) AS periodicTotal FROM periodic_requests perReq WHERE perReq.address_id IN(" +
                                                                                                            " SELECT ad.address_id FROM addresses ad WHERE ad.user_id = "+id+")";
                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                content = new List<Phoenix_Tracing.Models.TotalWork.Total>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.TotalWork.Total>((Func<DataRow, Phoenix_Tracing.Models.TotalWork.Total>)(Lot => new Phoenix_Tracing.Models.TotalWork.Total()
                    {
                        //Sql sorguda count Int64 biçiminde gelmektedir.
                        total = Lot.Field<Int64>("total")

                    })).ToList<Phoenix_Tracing.Models.TotalWork.Total>();
                }

                return Json(content, JsonRequestBehavior.AllowGet);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
        #endregion

        // Çıkış yaparken oturum bilgilerini sıfırlar.
        [BasicAuth]
        public ActionResult LoginOut()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Login");
        }

        //[BasicAuth]
        //public ActionResult AfterLogin()
        //{
        //    if (Session["UserID"] != null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}
    }
}