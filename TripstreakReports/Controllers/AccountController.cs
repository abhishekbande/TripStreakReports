using System.Configuration;
using System.Web.Mvc;
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
                    Session["IsUserLoggedIn"] = "True";

                    return RedirectToAction("Home", "Dashboard");
                }
            }

            model.Message = "The user name or password provided is incorrect.";            

            return View(model);
        }
        
        public ActionResult LogOut()
        {            
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();        
            return RedirectToAction("Login", "Account");
        }
    }
}
