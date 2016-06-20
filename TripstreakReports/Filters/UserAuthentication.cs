using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TripstreakReports.Filters
{
    public class UserAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // check  sessions here
            if (HttpContext.Current.Session["IsUserLoggedIn"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}