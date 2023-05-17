using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phoenix_Tracing.Models
{
    //VT Bağlantısını yapar
    public class DEF_DB
    {
        //public static string DbServerAdress = "localhost";
        public static string DbServerAdress = "94.73.149.62";
        public static string DbName = "phoenixtracing";
        public static string DbUserName = "SamedZZZZ";
        public static string DbPassword = "samed1234";

        public readonly static string DbConnectionString = $"Server={DbServerAdress};Database={DbName};Uid={DbUserName};Pwd={DbPassword};SslMode=Preferred;Convert Zero Datetime=true; Allow Zero Datetime=true;";
    }
}