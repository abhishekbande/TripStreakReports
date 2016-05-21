using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using TripstreakReports.DatAccess;
using TripstreakReports.Models;
using System.Data.Entity;

namespace TripstreakReports.Controllers
{    
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/        
        private DbContext _dbContext;

        [HttpGet]
        public ActionResult Home()
        {
            DashboardModel model = new DashboardModel();
            model.IsProdEnvironemt = false;
            ViewBag.MessageAlert = TempData["Message"];
            _dbContext = GetDbContext(model.IsProdEnvironemt);
            CommonContentEntities commonContent = new CommonContentEntities();
            model.Amenities = commonContent.airamenities.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Home(DashboardModel model)
        {
            if (model != null)
            {
                _dbContext = GetDbContext(model.IsProdEnvironemt);
                CommonContentEntities commonContent = new CommonContentEntities();
                model.Amenities = commonContent.airamenities.ToList();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddNewAmenity(DashboardModel model)
        {
            if (model != null)
            {
                //CommonContentEntities commonContent = new CommonContentEntities();
                //commonContent.airamenities.Add(model.Amenity);
                //commonContent.SaveChanges();
                TempData["Message"] = "Success! Amenity added successfully.";     
            }

            return RedirectToAction("Home", "Dashboard");
        }

        [HttpPost]
        public ActionResult UpdateAmenity(DashboardModel model)
        {
            if (model != null)
            {
                //CommonContentEntities commonContent = new CommonContentEntities();
                //commonContent.airamenities.Add(model.Amenity);
                //commonContent.SaveChanges();
                TempData["Message"] = "Success! Amenity Updated successfully.";
            }

            return RedirectToAction("Home", "Dashboard");
        }

        private DbContext GetDbContext(bool isProdEnvironment)
        {
            DbContext dbContext;

            if (isProdEnvironment)
            {
                dbContext =
                    new DbContext(ConfigurationManager.ConnectionStrings["ProdCommonContentEntities"].ConnectionString);
            }
            else
            {
                dbContext = new DbContext(ConfigurationManager.ConnectionStrings["StageCommonContentEntities"].ConnectionString);
            }

            return dbContext;
        }

    }
}
