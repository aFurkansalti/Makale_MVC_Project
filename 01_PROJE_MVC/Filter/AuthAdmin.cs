using _01_PROJE_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace _01_PROJE_MVC.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter      // Login olmuş mu olmamış mı diye kontrol eden filtre
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionUser.Login != null && SessionUser.Login.Admin == false)
            {
                filterContext.Result = new RedirectResult("/Home/YetkisizErisim");  // Nereye yönlendirmesi gerekir
            }
        }
    }
}