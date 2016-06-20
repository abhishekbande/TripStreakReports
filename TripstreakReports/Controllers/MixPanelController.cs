using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripstreakReports.Models;

namespace TripstreakReports.Controllers
{
    public class MixPanelController : Controller
    {
        //
        // GET: /MixPanel/

        public ActionResult Home()
        {
            return View(new MixpanelModel(){IsProdEnvironemt = false});
        }

    }
}
