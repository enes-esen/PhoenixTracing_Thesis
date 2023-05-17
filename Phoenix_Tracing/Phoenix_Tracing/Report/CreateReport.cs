using MySql.Data.MySqlClient;
using MySql.Data.Types;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;

namespace Phoenix_Tracing.Report
{
    public class CreateReport
    {
        public enum ReportType
        {
            Periodic,
            Operationel
        }
        private string[] parts = new string[6];

        int requestID;
        int[] job_ids;
        ReportType reportType;
        DataTable request = new DataTable();
        DataTable jobs = new DataTable();
        DataTable steps = new DataTable();
        DataTable breaks = new DataTable();

        public CreateReport(int request_id, ReportType report_type)
        {
            requestID = request_id;
            reportType = report_type;
            GetHTMLParts();
            GetTables(report_type == ReportType.Periodic ? "periodic_" : "operation_");
        }

        public void GetTables(string type)
        {
            string request_query = @"SELECT DISTINCT req.request_id
	                                        ,req.name AS request_name
	                                        ,req.detail AS request_detail
	                                        ,req.date AS request_date
	                                        ,reqT.name AS request_type_name
	                                        ,reqS.name AS  request_status_name
	                                        ,a.name AS address_name
	                                        ,a.city AS address_city
	                                        ,a.phone AS address_phone
	                                        ,a.address AS address
                            FROM "+type+"requests req " +
                                ","+type+"request_types reqT " +
                                ","+type+"request_status reqS " +
                                ",addresses a " +
                                "WHERE req.request_type_id = reqT.type_id " +
                                    " AND req.status_id = reqS.status_id " +
                                    " AND req.request_id = "+requestID+"";

            request = CustomTableQuery(request_query);

            string jobs_query = @"SELECT job.job_id 
                                          ,job.request_id
                                          ,u.name AS t_name
                                          ,u.surname AS t_surname
                                          ,u.phone AS t_phone
                                          ,job.name
                                          ,job.detail
                                          ,job.start_date
                                          ,job.total_work_time
                                          ,IF(end_date = '0000-00-00 00:00:00', '0000-00-00 00:00:00', end_date) AS end_date
                                          ,jobS.name AS job_status
                                  FROM "+type+"jobs job " +
                                      ","+type+"job_status jobS " +
                                      ",users u " +
                                  "WHERE job.technician_id = u.user_id" +
                                  "  AND job.status_id = jobS.status_id" +
                                  "  AND request_id= " + request.Rows[0]["request_id"].ToString();

            jobs = CustomTableQuery(jobs_query);
            job_ids = GetJobIDs(jobs);

            string steps_query = "SELECT job_id, step, done_time, done FROM " + type + "steps ";
            for (int i = 0; i < job_ids.Length; i++)
            {
                steps_query += i == 0 ? " WHERE " : " OR ";
                steps_query += " job_id = " + job_ids[i].ToString();
            }
            steps = CustomTableQuery(steps_query);

            string breaks_query = "SELECT job_id, break_start, break_end, total_break_time FROM "
                + type + "job_breaks ";
            for (int i = 0; i < job_ids.Length; i++)
            {
                breaks_query += i == 0 ? " WHERE " : " OR ";
                breaks_query += " job_id = " + job_ids[i].ToString();
            }
            breaks = CustomTableQuery(breaks_query);

        }

        int[] GetJobIDs(DataTable jobs)
        {
            int[] ids = new int[jobs.Rows.Count];

            for (int i = 0; i < jobs.Rows.Count; i++)
            {
                ids[i] = Convert.ToInt32(jobs.Rows[i]["job_id"].ToString());
            }

            return ids;
        }

        public DataTable CustomTableQuery(string sqlString)
        {
            try
            {
                MySqlConnection connection = ConnectionGenerator();
                DataTable dataTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand();

                command.CommandText = sqlString;
                command.Connection = connection;
                adapter.SelectCommand = command;

                adapter.Fill(dataTable);
                connection.Close();

                return dataTable;
            }
            catch (Exception ex)
            {
                var aaa = ex;
                return null;
            }
        }

        public MySqlConnection ConnectionGenerator()
        {
            MySqlConnection connection = new MySqlConnection(Phoenix_Tracing.Models.DEF_DB.DbConnectionString);
            try
            {
                connection.Open();
            }
            catch (Exception err) { }

            return connection;
        }

        private void GetHTMLParts()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            for (int i = 1; i <= 6; i++)
            {
                var webRequest = WebRequest.Create(@"https://phoenixtracing.com/desktop_src/part_0" + i.ToString() + ".txt");

                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    parts[i - 1] = reader.ReadToEnd();
                }
            }
        }

        private DataTable StepAndBreakeCreator(string job_id, DataTable steps_, DataTable breaks_)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("step_name");
            dataTable.Columns.Add("step_date", typeof(DateTime));
            dataTable.Columns.Add("step_status");

            for (int i = 0; i < steps_.Rows.Count; i++)
            {
                if (steps_.Rows[i]["job_id"].ToString() != job_id) continue;
                DataRow dataRow = dataTable.NewRow();
                dataRow["step_name"] = steps_.Rows[i]["step"].ToString();
                //dataRow["step_date"] = DateTime.Parse(steps_.Rows[i]["done_time"].ToString());

                var step_date = steps_.Rows[i]["done_time"].ToString();
                string validFormats = "MM/dd/yyyy HH:mm:ss";
                CultureInfo provider = new CultureInfo("en-US");
                DateTime result = DateTime.ParseExact(step_date, validFormats, provider);
                dataRow["step_date"] = result;
                
                dataRow["step_status"] = steps_.Rows[i]["done"].ToString() == "1" ? "Tamamlandı" : "Tamamlanmadı";
                dataTable.Rows.Add(dataRow);
            }
            for (int i = 0; i < breaks_.Rows.Count; i++)
            {
                if (breaks_.Rows[i]["job_id"].ToString() != job_id) continue;

                string validFormats = "MM/dd/yyyy HH:mm:ss";
                CultureInfo provider = new CultureInfo("en-US");

                DataRow dataRowS = dataTable.NewRow();
                dataRowS["step_name"] = "Mola";
                var breakStart = breaks_.Rows[i]["break_start"].ToString();
                DateTime result_breakStart = DateTime.ParseExact(breakStart, validFormats, provider);
                dataRowS["step_date"] = result_breakStart;
                dataRowS["step_status"] = "Başlangıç";
                dataTable.Rows.Add(dataRowS);
                                
                DataRow dataRowE = dataTable.NewRow();
                dataRowE["step_name"] = "Mola";
                var breakEnd = breaks_.Rows[i]["break_end"].ToString();
                DateTime result_breakEnd = DateTime.ParseExact(breakEnd, validFormats, provider);
                dataRowE["step_date"] = result_breakEnd;
                dataRowE["step_status"] = "Bitiş";
                dataTable.Rows.Add(dataRowE);
            }
            DataView dataView = dataTable.DefaultView;
            dataView.Sort = "step_date asc";
            DataTable sortedDataTable = dataView.ToTable();
            return sortedDataTable;            
        }

        public void Creator(string userNameSurname)
        {
            string text_total = parts[0];
            text_total = text_total.Replace("__pdf_request_time__", DateTime.Now.Date.ToShortDateString());
            text_total = text_total.Replace("__request_name__", request.Rows[0]["request_name"].ToString());
            text_total = text_total.Replace("__request_type__", reportType == ReportType.Operationel ? "Operasyonel" : "Periyodik");
            text_total = text_total.Replace("__request_owner__", userNameSurname);
            text_total = text_total.Replace("__address__", request.Rows[0]["address_name"] + " " +
                request.Rows[0]["address_city"] + " " + request.Rows[0]["address"] + " Phone: " + request.Rows[0]["address_phone"]);
            text_total = text_total.Replace("__request_date__", request.Rows[0]["request_date"].ToString());
            text_total = text_total.Replace("__request_name__", request.Rows[0]["request_name"].ToString());
            text_total = text_total.Replace("__request_detail__", request.Rows[0]["request_detail"].ToString());
            text_total = text_total.Replace("__request_status__", request.Rows[0]["request_status_name"].ToString());

            for (int i = 0; i < job_ids.Length; i++)
            {
                text_total += parts[1];
                text_total = text_total.Replace("__job_count__", (i + 1).ToString() + ". Tanım");
                text_total = text_total.Replace("__technician_name__", jobs.Rows[i]["t_name"] + " " + jobs.Rows[i]["t_surname"]);
                text_total = text_total.Replace("__technician_phone__", jobs.Rows[i]["t_phone"].ToString());
                text_total = text_total.Replace("__job_status__", jobs.Rows[i]["job_status"].ToString());
                text_total = text_total.Replace("__job_start_date__", jobs.Rows[i]["start_date"].ToString().Split(' ')[0]);
                text_total = text_total.Replace("__job_end_date__",
                    jobs.Rows[i]["end_date"].ToString().Split(' ')[0] == "00.00.0000"
                    ? "-" : jobs.Rows[i]["end_date"].ToString().Split(' ')[0]);

                text_total = text_total.Replace("__total_job_time__",
                    jobs.Rows[i]["total_work_time"].ToString() == "00:00:00"
                    ? "-" : jobs.Rows[i]["total_work_time"].ToString());
                text_total += parts[2];

                DataTable steps_breaks = StepAndBreakeCreator(jobs.Rows[i]["job_id"].ToString(), steps, breaks);
                for (int j = 0; j < steps_breaks.Rows.Count; j++)
                {
                    text_total += parts[3];
                    text_total = text_total.Replace("__step_name__", steps_breaks.Rows[j]["step_name"].ToString());
                    text_total = text_total.Replace("__step_date__",
                        steps_breaks.Rows[j]["step_date"].ToString() == "1.01.0001 00:00:00"
                        ? "-" : steps_breaks.Rows[j]["step_date"].ToString());
                    text_total = text_total.Replace("__step_status__", steps_breaks.Rows[j]["step_status"].ToString());
                }
                text_total += parts[4];
            }
            text_total += parts[5];

            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(text_total);
            CreatePDF(doc);
            doc.Close();
        }

        private void CreatePDF(PdfDocument doc)
        {            
            string report_type = reportType == ReportType.Periodic ? "periodic_" : "operation_";
            
            doc.Save(Environment.ExpandEnvironmentVariables(@"%HOMEDRIVE%%HOMEPATH%/Downloads/" + report_type + "rapor.pdf"));
        }
    }
}