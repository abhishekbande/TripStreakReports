using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using TripstreakReports.DatAccess;
using TripstreakReports.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.IO;
using System.Text;
using System.Web.Services.Description;

namespace TripstreakReports.Controllers
{    
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/                
        private static CommonContentEntities _commonContent;

        private static bool _isProdEnvironment=true;

        [HttpGet]
        public ActionResult Home()
        {               
            ViewBag.MessageAlert = TempData["Message"];

            ChangeDatabase(_isProdEnvironment);

            DashboardModel model = GetDashboardModel();            

            return View(model);
        }

        private DashboardModel GetDashboardModel()
        {
            return new DashboardModel()
            {
                Amenities = _commonContent.airamenities.ToList(),
                IsProdEnvironemt = _isProdEnvironment,
            };
        }

        [HttpPost]
        public ActionResult Home(DashboardModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteAmenities(DashboardModel model)
        {
            if (model != null && !string.IsNullOrEmpty(model.AmenityIds))
            {
                try
                {
                    List<string> amenityIdList = model.AmenityIds.Split(',').ToList();

                    amenityIdList.RemoveAll(x => string.IsNullOrEmpty(x));

                    List<int> amenityIds = amenityIdList.Select(x => x.Trim()).Select(y => Convert.ToInt32(y)).ToList();

                    if (amenityIds.Count > 0)
                    {
                        foreach (int id in amenityIds)
                        {
                            airamenity amenity = _commonContent.airamenities.Find(id);
                            _commonContent.airamenities.Remove(amenity);
                        }
                    }

                    _commonContent.SaveChanges();

                    TempData["Message"] = "Success! Amenity deleted successfully.";   
                }
                catch (Exception exception)
                {
                    TempData["Message"] = "Error! " + exception.Message;          
                }               
            }
            return RedirectToAction("Home", "Dashboard");
        }        

        [HttpPost]
        public ActionResult AddNewAmenity(DashboardModel model)
        {
            if (model != null)
            {
                bool otherEnvironment = false;

                bool currentEnvironment = this.AddAmenity(model);

                if (model.AddAmenityToOtherEnv)
                {
                    this.ChangeDatabase(!_isProdEnvironment);

                    otherEnvironment = this.AddAmenity(model);

                    this.ChangeDatabase(_isProdEnvironment);
                }

                if (model.AddAmenityToOtherEnv)
                {
                    if (currentEnvironment && otherEnvironment)
                    {
                       TempData["Message"] = "Success! Amenity added successfully on Stage and Prod Environment.";
                    }
                    else if (currentEnvironment)
                    {
                        TempData["Message"] = "Success! Amenity added successfully on " +
                                              (_isProdEnvironment ? "Prod" : "Stage") + "Environment, but failed on " +
                                              ((!_isProdEnvironment) ? "Prod" : "Stage");
                    }
                    else if (otherEnvironment)
                    {
                        TempData["Message"] = "Success! Amenity added successfully on " +
                                              ((!_isProdEnvironment) ? "Prod" : "Stage") + "Environment, but failed on " +
                                              (_isProdEnvironment ? "Prod" : "Stage");
                    }
                }
                else
                {
                    TempData["Message"] = "Success! Amenity added successfully.";
                }
            }
            return base.RedirectToAction("Home", "Dashboard");
        }

        private bool AddAmenity(DashboardModel model)
        {
            bool result;
            try
            {
                _commonContent.airamenities.Add(model.Amenity);
                _commonContent.SaveChanges();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        [HttpPost]
        public ActionResult UpdateAmenity(DashboardModel model)
        {
            if (model != null)
            {
                try
                {
                    ChangeDatabase(_isProdEnvironment);

                    airamenity existingAmenity = _commonContent.airamenities.FirstOrDefault(x=>x.ID==model.Amenity.ID);

                    if (existingAmenity != null)
                    {
                        existingAmenity.AircraftModel = model.Amenity.AircraftModel;
                        existingAmenity.AircraftType = model.Amenity.AircraftType;
                        existingAmenity.Airline = model.Amenity.Airline;
                        existingAmenity.AirlineCode = model.Amenity.AirlineCode;
                        existingAmenity.AvailableAtArrivalAirport = model.Amenity.AvailableAtArrivalAirport;
                        existingAmenity.AvailableAtDepartureAirport = model.Amenity.AvailableAtDepartureAirport;
                        existingAmenity.AvailableInCountry = model.Amenity.AvailableInCountry;
                        existingAmenity.BeingUpgraded = model.Amenity.BeingUpgraded;
                        existingAmenity.Cabin = model.Amenity.Cabin;
                        existingAmenity.IataTypeMapping = model.Amenity.IataTypeMapping;
                        existingAmenity.InSeatPowerType = model.Amenity.InSeatPowerType;
                        existingAmenity.Manufacturer = model.Amenity.Manufacturer;
                        existingAmenity.SeatType = model.Amenity.SeatType;
                        existingAmenity.VideoType = model.Amenity.VideoType;
                        existingAmenity.WiFiAvailablilty = model.Amenity.WiFiAvailablilty;

                        _commonContent.SaveChanges();
                        TempData["Message"] = "Success! Amenity Updated successfully.";
                    }
                    else
                    {
                        TempData["Message"] = "Error! Amenity not found.";
                    }                    
                }
                catch (Exception exception)
                {
                    TempData["Message"] = "Error! " + exception.Message;
                }
            }

            return RedirectToAction("Home", "Dashboard");
        }

        [HttpGet]
        public ActionResult SwitchEnvironment(bool isProdEnvironment)
        {            
            _isProdEnvironment = isProdEnvironment;            
                
             ChangeDatabase(_isProdEnvironment);

             DashboardModel model = new DashboardModel();

             model.Amenities = _commonContent.airamenities.ToList();

             model.IsProdEnvironemt = _isProdEnvironment;

             return PartialView("_AmenitiesTablePartial", model);
        }

        public FileContentResult ExportToCsv()
        {
            try
            {
                string csv = GetAsCsvAmenities(_commonContent.airamenities.ToList());

                return File(new UTF8Encoding().GetBytes(csv), "text/csv", "Amenities.csv");
            }
            catch (Exception exception)
            {
                
            }
            return null;
        }

        private string GetAsCsvAmenities(List<airamenity> amenities)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Write the header columns
            string header = GetCsvHeader();
            stringBuilder.AppendLine(header);

            //write every subscriber to the file
            foreach (airamenity amenity in amenities)
            {
                string amenityString =
                    string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                        amenity.ID,
                        amenity.Airline,
                        amenity.AirlineCode,
                        amenity.Manufacturer,
                        amenity.AircraftModel,
                        amenity.AircraftType,
                        amenity.IataTypeMapping,
                        amenity.Cabin,
                        amenity.VideoType,
                        amenity.InSeatPowerType,
                        amenity.WiFiAvailablilty,
                        amenity.SeatType,
                        amenity.AvailableAtArrivalAirport,
                        amenity.AvailableAtDepartureAirport,
                        amenity.AvailableInCountry,
                        amenity.BeingUpgraded,
                        amenity.LastUpdatedOn);

                stringBuilder.AppendLine(amenityString);
            }

            return stringBuilder.ToString();
        }

        private static string GetCsvHeader()
        {
            string header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                "ID", "Airline", "AirlineCode", "Manufacturer", "AircraftModel", "AircraftType", "IataTypeMapping",
                "Cabin", "VideoType", "InSeatPowerType", "WiFiAvailablilty", "SeatType",
                "AvailableAtArrivalAirport", "AvailableAtDepartureAirport", "AvailableInCountry", "BeingUpgraded",
                "LastUpdatedOn");
            return header;
        }


        [System.Web.Http.HttpPost]
        public ActionResult ImportCsv(HttpPostedFileBase postedFiles)
        {
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                string headerLine = GetCsvHeader();
                int expectedNumberOfColumns = headerLine.Split(',').Count();

                using (StreamReader reader = new StreamReader(file.InputStream))
                {
                    string csvFileContent = reader.ReadToEnd();

                    var lines = csvFileContent.Split(Environment.NewLine.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (string amenityCsvString in lines)
                    {
                        if (string.IsNullOrWhiteSpace(amenityCsvString)
                            || string.Equals(amenityCsvString, headerLine, StringComparison.InvariantCultureIgnoreCase))
                        // Skip header, if user has added it in the uploaded file
                        {
                            continue;
                        }

                        var columnValues = amenityCsvString.Split(',');
                        airamenity airamenity = new airamenity();

                        if (columnValues.Count() != expectedNumberOfColumns)
                        {
                            throw new Exception("Incorrect number of columns in the csv. Expected " +
                                                expectedNumberOfColumns + " columns of data.");
                        }

                        airamenity.Airline = columnValues[1];
                        airamenity.AirlineCode = columnValues[2];
                        airamenity.Manufacturer = columnValues[3];
                        airamenity.AircraftModel = columnValues[4];
                        airamenity.AircraftType = columnValues[5];
                        airamenity.IataTypeMapping = columnValues[6];
                        airamenity.Cabin = columnValues[7];
                        airamenity.VideoType = columnValues[8];
                        airamenity.InSeatPowerType = columnValues[9];
                        airamenity.WiFiAvailablilty = columnValues[10];
                        airamenity.SeatType = columnValues[11];
                        airamenity.AvailableAtArrivalAirport = columnValues[12];
                        airamenity.AvailableAtDepartureAirport = columnValues[13];
                        airamenity.AvailableInCountry = columnValues[14];
                        airamenity.BeingUpgraded = columnValues[15];

                        _commonContent.airamenities.Add(airamenity);
                    }
                }

                _commonContent.SaveChanges();

                TempData["Message"] = "Success! Amenities imported successfully.";
            }
            catch (Exception exception)
            {
                TempData["Message"] = "Error! " + exception.Message;
            }
            return RedirectToAction("Home", "Dashboard");
        }

        private void ChangeDatabase(bool isProdEnvironment)
        {                               
            if (isProdEnvironment)
            {
                _commonContent = new CommonContentEntities();
            }
            else
            {
                _commonContent = new CommonContentEntities("StageCommonContentEntities");
            }
        }
    }
}
