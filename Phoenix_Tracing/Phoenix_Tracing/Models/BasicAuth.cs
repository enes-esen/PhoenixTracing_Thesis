using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Phoenix_Tracing.Models;

namespace Phoenix_Tracing.Models
{
    public class BasicAuth : AuthorizeAttribute
    {
        // Kullanıcı girişi kontrol eder ve çıkış sonrası geri dönüşü engeller
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                Users _User = (Users)HttpContext.Current.Session["BasicAuth"];

                if (_User != null && _User.REF > 0)
                {
                    return true;
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/Login/Index");
                    return false;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("/Home/LoginOut");
                return false;
            }
        }
    }
}




