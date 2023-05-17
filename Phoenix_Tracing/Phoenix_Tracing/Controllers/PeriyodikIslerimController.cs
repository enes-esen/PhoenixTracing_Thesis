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
    public class PeriyodikIslerimController : Controller
    {
        [BasicAuth]
        public ActionResult Index()
        {
            return View();
        }
        
        //Periyodik işlerim listesini vt'den getirir.
        [BasicAuth]
        public ActionResult PeriyodikIslerim()
        {
            int user_id = Convert.ToInt32(Session["UserID"]);
            List<Phoenix_Tracing.Models.PeriyodikIslerim> content = null;
            string query = @"SELECT DISTINCT perJ.job_id
                            		,perR.request_id
                            		,perJ.name  AS per_job_name
                            		,perJ.detail AS per_job_detail
                            		,perJ.status_id
                            		,perJS.name AS per_job_status_name
                            		,a.address_id
                            		,a.name AS address_name
                            		,a.phone AS address_phone
                            		,perR.period AS per_request_period
                            		,perR.date AS per_request_date
                            		,ADDDATE(perR.date, INTERVAL perR.period MONTH) AS per_request_afterDate
                            		,perR.request_type_id
                            		,perRT.name  AS per_request_type_name
                            FROM periodic_jobs perJ
                            	,periodic_job_status perJS
                            	,periodic_requests perR
                            	,periodic_request_types perRT
                            	,addresses a
                            	,users u
                            WHERE perJ.request_id = perR.request_id
                            	AND perJ.status_id = perJS.status_id
                            	AND perJ.address_id = a.address_id
                            	AND perR.request_type_id = perRT.type_id
                            	AND a.user_id = u.user_id
                            	AND u.user_id = " + user_id+"" +
                                " ORDER BY job_id";           

            try
            {
                DataSet dt = MySqlHelper.ExecuteDataset(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                
                content = new List<PeriyodikIslerim>();
                if (dt != null && dt.Tables[0] != null && dt.Tables.Count > 0)
                {
                    content = dt.Tables[0].AsEnumerable().Select<DataRow, Phoenix_Tracing.Models.PeriyodikIslerim>((Func<DataRow, Phoenix_Tracing.Models.PeriyodikIslerim>)(Lot => new Phoenix_Tracing.Models.PeriyodikIslerim()
                    {
                        job_id = Lot.Field<int>("job_id"),
                        request_id = Lot.Field<int>("request_id"),

                        per_job_name = Lot.Field<string>("per_job_name"),
                        per_job_detail = Lot.Field<string>("per_job_detail"),

                        status_id = Lot.Field<int>("status_id"),
                        per_job_status_name = Lot.Field<string>("per_job_status_name"),

                        address_name = Lot.Field<string>("address_name"),
                        address_phone = Lot.Field<string>("address_phone"),

                        per_request_period = Lot.Field<int>("per_request_period"),
                        per_request_date = Lot.Field<MySqlDateTime>("per_request_date"),
                        per_request_afterDate = Lot.Field<MySqlDateTime>("per_request_afterDate"),

                        request_type_id = Lot.Field<int>("request_type_id"),
                        per_request_type_name = Lot.Field<string>("per_request_type_name")
                    })).ToList<Phoenix_Tracing.Models.PeriyodikIslerim>();
                }

                MySqlDataReader rdr = MySqlHelper.ExecuteReader(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query);
                List<int> jobID_list = new List<int>();

                while (rdr.Read())
                {
                    jobID_list.Add(Convert.ToInt32(rdr["job_id"]));
                }

                return PartialView("PeriyodikIslerim", content);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        //Periyodik iş için yapılan ve yapılacak listesinin vt'den getirilmesi
        public int PeriodicJobID(int jobID)
        {
            return jobID;
        }
    }
}