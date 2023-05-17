using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MySql.Data;
using MySql.Data.MySqlClient;
using Phoenix_Tracing.Models;

namespace Phoenix_Tracing.Controllers
{
    //Kullanıcı girişi ve kayıt olma sayfası
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //Kullanıcı giriş kontrolünü yapar.
        [HttpPost]
        public JsonResult CheckValidUser(Users model)
        {
            string query = @"SELECT
                                u.*
                                ,t.name AS user_type_name
                                ,COUNT(u.user_id) AS REF
                            FROM
                                users u
                                ,user_types t
                            WHERE
                                u.user_type_id = t.type_id
                            AND mail = @Email
                            AND password = @Password
                            AND active = 1;";
            try
            {
                if (model.password != null & model.mail != null)
                {
                    model.password = HashGenerator(model.password);
                }

                Result rst = new Result();
                rst.Ref = 0;

                MySqlParameter[] parameters = new MySqlParameter[2];
                parameters[0] = new MySqlParameter("@Email", model.mail);
                parameters[1] = new MySqlParameter("@Password", model.password);

                MySqlDataReader rdr = MySqlHelper.ExecuteReader(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query, parameters);

                if (rdr.Read())
                {
                    model.REF = Convert.ToInt32(rdr["REF"]);
                    rst.Ref = model.REF;

                    if (rst.Ref == 1)
                    {
                        model.user_id = Convert.ToInt32(rdr["user_id"]);
                        model.mail = rdr["mail"].ToString();
                        model.password = rdr["password"].ToString();
                        model.user_type_id = Convert.ToInt32(rdr["user_type_id"]);
                        model.user_type_name = rdr["user_type_name"].ToString();
                        model.name = rdr["name"].ToString();
                        model.surname = rdr["surname"].ToString();
                        model.phone = rdr["phone"].ToString();
                        model.active = Convert.ToInt32(rdr["active"]);

                        Session["UserID"] = model.user_id.ToString();
                        Session["mail"] = model.mail;
                        Session["UserName"] = model.name;
                        Session["UserSurname"] = model.surname;
                        Session["UserPhone"] = model.phone;
                        Session["UserTypeID"] = model.user_type_id.ToString();
                        Session["UserTypeName"] = model.user_type_name;
                        Session["UserActive"] = model.active.ToString();
                        Session["UserREF"] = model.REF.ToString();

                        Session["BasicAuth"] = model;

                        rst.Message = "Giriş Başarılı";
                    }
                    else
                    {
                        rst.Message = "Hatalı giriş";
                    }
                    return Json(rst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    rst.Message = "Hatalı giriş";
                    return Json(rst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (MySqlException ex)
            {
                Result rst = new Result();
                rst.Message = ex.Message;
                return Json(rst, JsonRequestBehavior.AllowGet);
            }
        }

        //Kullanıcı kayıt girişlerini vt'ye gönderir
        [HttpPost]
        public JsonResult Register(string mail, string pass, string name, string surname, string phone)
        {
            try
            {
                string reg_name = name;
                string reg_surname = surname;
                string reg_mail = mail;
                string reg_pass = HashGenerator(pass);
                string reg_phone = phone;

                string query = @"INSERT INTO users(mail
                                                ,password
                                                ,user_type_id
                                                ,name
                                                ,surname
                                                ,phone
                                                ,active
                                                )
                                    VALUES(@REG_MAIL
                                    ,@REG_PASS
                                    ,5
                                    ,@REG_NAME
                                    ,@REG_SURNAME
                                    ,@REG_PHONE
                                    ,1)";

                Users item = new Users();

                item.name = reg_name;
                item.surname = reg_surname;
                item.mail = reg_mail;
                item.password = reg_pass;
                item.phone = reg_phone;

                MySqlParameter[] parameters = new MySqlParameter[5];
                parameters[0] = new MySqlParameter("@REG_MAIL", item.mail);
                parameters[1] = new MySqlParameter("@REG_PASS", item.password);
                parameters[2] = new MySqlParameter("@REG_NAME", item.name);
                parameters[3] = new MySqlParameter("@REG_SURNAME", item.surname);
                parameters[4] = new MySqlParameter("@REG_PHONE", item.phone);

                var rdr = MySqlHelper.ExecuteNonQuery(Phoenix_Tracing.Models.DEF_DB.DbConnectionString, query, parameters);

                return Json(rdr, JsonRequestBehavior.AllowGet);
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        //Şifreleri Hashleme fonksiyonu
        public string HashGenerator(string pwd)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }
    }
}