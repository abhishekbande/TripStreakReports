using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmenityUpdator.UI.Controllers;
using AmenityUpdator.UI.Helper;
using TripstreakReports.Filters;
using TripstreakReports.Helper;
using TripstreakReports.Models;
using Filter = TripstreakReports.Models.Filter;

namespace TripstreakReports.Controllers
{
    public class MixPanelController : Controller
    {
        //
        // GET: /MixPanel/
        private static bool _isProdEnvironment = true;

        private static MixpanelKeys _mixpanelKeys;

        [HttpGet]
        [UserAuthentication]
        public ActionResult Home()
        {
            ViewBag.MessageAlert = TempData["Message"];

            ChangeMixpanelKeys();

            MixpanelModel model = GetMixpanelModel();

            return View(model);
        }

        [HttpPost]
        [UserAuthentication]
        public ActionResult Home(MixpanelModel model)
        {
            ChangeMixpanelKeys();

            model = GetMixpanelModel();

            return View(model);
        }

        [HttpPost]
        [UserAuthentication]
        public ActionResult DownloadPreconfiguredReport(MixpanelModel model)
        {            
            try
            {
                MixpanelDataExportConnector mixpanelDataExport = new MixpanelDataExportConnector(_mixpanelKeys);

                switch (model.PreconfiguredReports)
                {
                    case "UserFfpRanks":
                        string userFfpRankResponse = mixpanelDataExport.GetUserFfpRankReport();
                        ExportResultToCsv(userFfpRankResponse, ExportFileNameHelper.UserFfpProgramPreferenceRank);
                        break;
                    case "UserCountWithFfpRank1":

                        string userCountWithFfpAsRankOneResponse = mixpanelDataExport.GetUserCountWithFfpAsRankOneReport();
                        ExportResultToCsv(userCountWithFfpAsRankOneResponse, ExportFileNameHelper.UserWithFfpRank1);

                        break;
                    case "Top10FfpPrograms":
                        string topTenFfpUserResponse = mixpanelDataExport.GetTopTenFfpUserPrograms();
                        ExportResultToCsv(topTenFfpUserResponse, ExportFileNameHelper.TopTenFfpPrograms);
                        break;

                    default:
                        TempData["Message"] = "Error! Please select Preconfigured report to download.";
                        break;
                }
            }
            catch
            {
                TempData["Message"] = "Error! Error occured while downloading data.";

                return RedirectToAction("Home", "MixPanel");
            }
            return PartialView("_MixpanelReportPanels",GetMixpanelModel());
        }

        [HttpPost]
        [UserAuthentication]
        public ActionResult DownloadSegmentReport(MixpanelModel model)
        {
            try
            {
                MixpanelDataExportConnector mixpanelDataExport = new MixpanelDataExportConnector(_mixpanelKeys);

                ReportParameter reportParameter = SegmentReportsHelper.PopulateSegmentReportParameter(model);

                string rawReportResponse = mixpanelDataExport.GetSegmentReport(reportParameter);

                ExportResultToCsv(rawReportResponse, SegmentReportsHelper.GetFileName(model.SegmentReports.IncludeUserInformation));
            }
            catch
            {
                TempData["Message"] = "Error! Error occured while downloading segment report data.";

                return RedirectToAction("Home", "MixPanel");
            }

            return PartialView("_MixpanelReportPanels", GetMixpanelModel());
        }

        [HttpPost]
        [UserAuthentication]
        public ActionResult DownloadCustomReport(MixpanelModel model)
        {
            try
            {
                MixpanelDataExportConnector mixpanelDataExport = new MixpanelDataExportConnector(_mixpanelKeys);

                ReportParameter customReportParameters = CustomReportHelper.PopulateCustomReportParameter(model);

                switch (model.CustomReports.ReportType)
                {
                    case "UserFfpRanks":

                        string topTenFfpUserResponse = CustomReportHelper.PopulateUserFfpRankReport(mixpanelDataExport, customReportParameters);
                        ExportResultToCsv(topTenFfpUserResponse, ExportFileNameHelper.TopTenFfpPrograms);
                        break;

                    case "UserCountWithFfpRank1":

                        string userCountWithFfpAsRankOneResponse = CustomReportHelper.PopulateCountOfUserFfpRankOneReport(mixpanelDataExport, customReportParameters);
                        ExportResultToCsv(userCountWithFfpAsRankOneResponse, ExportFileNameHelper.UserWithFfpRank1);
                        break;

                    case "Top10FfpPrograms":

                        string userFfpRankResponse = CustomReportHelper.PopulateTopTenFffpProgramReport(mixpanelDataExport, customReportParameters);
                        ExportResultToCsv(userFfpRankResponse, ExportFileNameHelper.UserFfpProgramPreferenceRank);
                        break;
                  
                    default:                        
                        string adhocReportResponse = mixpanelDataExport.GetAdhocReport(customReportParameters);
                        ExportResultToCsv(adhocReportResponse, ExportFileNameHelper.AdhocPrograms);
                        break;
                }
            }
            catch
            {
                TempData["Message"] = "Error! Error occured while downloading custom report data.";

                return RedirectToAction("Home", "MixPanel");
            }
            
            return PartialView("_MixpanelReportPanels", GetMixpanelModel());
        }

        private void ExportResultToCsv(string response, string fileName)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
            Response.ContentType = "text/csv";
            Response.Write(response);
            Response.End();
        }
             
        [HttpGet]
        public ActionResult SwitchEnvironment(bool isProdEnvironment)
        {
            _isProdEnvironment = isProdEnvironment;

            ChangeMixpanelKeys();

            MixpanelModel model = GetMixpanelModel();            

            model.IsProdEnvironemt = _isProdEnvironment;

            return PartialView("_MixpanelReportPanels", model);            
        }

        private void ChangeMixpanelKeys()
        {
            _mixpanelKeys = new MixpanelKeys();

            if (_isProdEnvironment)
            {
                _mixpanelKeys.ApiKey = ConfigurationManager.AppSettings["MixpanelApiKey"];
                _mixpanelKeys.ApiSecret = ConfigurationManager.AppSettings["MixpanelApiSecret"];
                _mixpanelKeys.Token = ConfigurationManager.AppSettings["MixpanelToken"];
            }
            else
            {
                _mixpanelKeys.ApiKey = ConfigurationManager.AppSettings["MixpanelBetaApiKey"];
                _mixpanelKeys.ApiSecret = ConfigurationManager.AppSettings["MixpanelBetaApiSecret"];
                _mixpanelKeys.Token = ConfigurationManager.AppSettings["MixpanelBetaToken"];
            }            
        }

        private static MixpanelModel GetMixpanelModel()
        {
            return new MixpanelModel
            {
                IsProdEnvironemt = _isProdEnvironment,
                SegmentReports = new SegmentReports
                {
                    DateTimeRange = string.Format("{0} - {1}", DateTime.Now.AddDays(-7).Date, DateTime.Now.Date),
                    IncludeUserInformation = false
                },
                CustomReports = new CustomReports(),
            };
        }

       
    }
}
