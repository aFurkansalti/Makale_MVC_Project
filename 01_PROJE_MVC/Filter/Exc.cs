using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _01_PROJE_MVC.Filter
{
    public class Exc : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Controller.TempData["hatalar"] = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult("/Home/HataliIslem");
        }
    }
}