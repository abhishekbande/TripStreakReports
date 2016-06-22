using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmenityUpdator.UI.Controllers;
using AmenityUpdator.UI.Helper;
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
        public ActionResult Home()
        {
            ChangeMixpanelKeys();

            MixpanelModel model = new MixpanelModel
            {
                IsProdEnvironemt = false,
                SegmentReports = new SegmentReports
                {
                    DateTimeRange = string.Format("{0} - {1}", DateTime.Now.Date, DateTime.Now.Date),
                    IncludeUserInformation = false
                }
            };
            return View(model);
        }

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
                TempData["Message"] = "Error! Error while downloading data.";
            }
            return PartialView("_MixpanelReportPanels");
        }


        public ActionResult DownloadSegmentReport(MixpanelModel model)
        {
            try
            {
                MixpanelDataExportConnector mixpanelDataExport = new MixpanelDataExportConnector(_mixpanelKeys);

                ReportParameter reportParameter = PopulateSegmentReportParameter(model);

                string rawReportResponse = mixpanelDataExport.GetSegmentReport(reportParameter);

                ExportResultToCsv(rawReportResponse, GetFileName(model.SegmentReports.IncludeUserInformation));
            }
            catch
            {
                TempData["Message"] = "Error! Error while downloading segment report data.";
            }            

            return PartialView("_MixpanelReportPanels");
        }

        private static string GetFileName(bool isUserInfoIncluded)
        {
            return isUserInfoIncluded ? ExportFileNameHelper.SegmentReportWithUserInfo : ExportFileNameHelper.SegmentReport;
        }

        private void ExportResultToCsv(string response, string fileName)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
            Response.ContentType = "text/csv";
            Response.Write(response);
            Response.End();
        }

        private static ReportParameter PopulateSegmentReportParameter(MixpanelModel mixpanelmodel)
        {
            ReportParameter customReportParameters = null;

            if (mixpanelmodel != null && mixpanelmodel.SegmentReports != null && !string.IsNullOrEmpty(mixpanelmodel.SegmentReports.DateTimeRange))
            {
                customReportParameters = new ReportParameter();

                string[] dateTimeRange = mixpanelmodel.SegmentReports.DateTimeRange.Split('-');

                if (dateTimeRange.Length > 0 && dateTimeRange.Length < 3)
                {
                    customReportParameters.ReportStartDate = Convert.ToDateTime(dateTimeRange[0]);

                    customReportParameters.ReportEndDate = Convert.ToDateTime(dateTimeRange[1]);

                    AdjustEndDateAsperPacificTimezone(customReportParameters);
                }
            }

            return customReportParameters;
        }

        //private static ReportParameter PopulateReportParameters(MixpanelModel mixpanelmodel)
        //{
        //    ReportParameter customReportParameters = new ReportParameter();

        //    //customReportParameters.OutputColumns = mixpanelmodel.SelectedOutputColumns;

        //    customReportParameters.ReportStartDate = Convert.ToDateTime(mixpanelmodel.SegmentReports.DateTimeRange.Split('-')[0]);
        //    customReportParameters.ReportEndDate = Convert.ToDateTime(mixpanelmodel.SegmentReports.DateTimeRange.Split('-')[1]);

        //    AdjustEndDateAsperPacificTimezone(customReportParameters);

        //    customReportParameters.IncludeUserInformation = mixpanelmodel.SegmentReportsIncludeUserInfo;

        //    //customReportParameters.Filter.Add(new Filter()
        //    //{
        //    //    SelectedProptery = mixpanelmodel.FilterSelectedProptery,
        //    //    SelectedOperator = mixpanelmodel.FilterSelectedOperator,
        //    //    SelectedValue = mixpanelmodel.FilterSelectedValue
        //    //});

        //    return customReportParameters;
        //}

        private static void AdjustEndDateAsperPacificTimezone(ReportParameter customReportParameters)
        {
            DateTime currentPacificTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            if (customReportParameters.ReportStartDate.Date > currentPacificTime.Date)
            {
                customReportParameters.ReportStartDate = currentPacificTime;
            }

            if (customReportParameters.ReportEndDate.Date > currentPacificTime.Date)
            {
                customReportParameters.ReportEndDate = currentPacificTime;
            }
        }

        [HttpGet]
        public ActionResult SwitchEnvironment(bool isProdEnvironment)
        {
            _isProdEnvironment = isProdEnvironment;

            ChangeMixpanelKeys();

            MixpanelModel model = new MixpanelModel();            

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
    }
}
