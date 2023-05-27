using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _01_PROJE_MVC.Models
{
    public class SessionUser
    {
        public static Kullanici Login { 
            get {
                if (HttpContext.Current.Session["login"] != null)
                {
                    return HttpContext.Current.Session["login"] as Kullanici;
                }
                return null;
            }
            set 
            {
                HttpContext.Current.Session["login"] = value;
            }
        }
    }
}