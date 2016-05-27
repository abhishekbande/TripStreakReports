using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using TripstreakReports.Filters;
using TripstreakReports.Models;

namespace TripstreakReports.Controllers
{     
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [HttpGet]
        public ActionResult Login()
        {            
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string password = ConfigurationManager.AppSettings["ApplicationLoginPassword"];

                if (model.Password.Equals(password))
                {
                    return RedirectToAction("Home", "Dashboard");
                }
            }

            model.Message = "The user name or password provided is incorrect.";            

            return View(model);
        }

    }
}
