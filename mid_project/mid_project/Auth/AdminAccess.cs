using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mid_project.Auth
{
    public class AdminAccess : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authenticated = base.AuthorizeCore(httpContext);
            if (!authenticated)
            {
                return false;
            }
            if (httpContext.Session["type"].ToString().Equals("Admin"))
            {
                return true;
            }
            return false;

            //return base.AuthorizeCore(httpContext);
        }
    }
}