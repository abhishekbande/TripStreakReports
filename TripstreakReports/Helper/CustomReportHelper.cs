using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmenityUpdator.UI.Controllers;
using AmenityUpdator.UI.Helper;
using TripstreakReports.Models;

namespace TripstreakReports.Helper
{
    public class CustomReportHelper
    {
        public static ReportParameter PopulateCustomReportParameter(MixpanelModel mixpanelmodel)
        {
            ReportParameter customReportParameters = null;

            if (mixpanelmodel != null && mixpanelmodel.CustomReports != null && !string.IsNullOrEmpty(mixpanelmodel.CustomReports.ReportType))
            {
                customReportParameters = new ReportParameter();

                customReportParameters.OutputColumns = mixpanelmodel.CustomReports.OutputColumns;

                customReportParameters.Filter = new Filter
                {
                    SelectedOperator = mixpanelmodel.CustomReports.SelectedOperator,
                    SelectedProptery = mixpanelmodel.CustomReports.FilterProperty,
                    SelectedValue = mixpanelmodel.CustomReports.Value
                };                
            }

            return customReportParameters;
        }

        public static string PopulateTopTenFffpProgramReport(MixpanelDataExportConnector mixpanelDataExport, ReportParameter customReportParameters)
        {            
            return mixpanelDataExport.GetTopTenFfpUserPrograms(customReportParameters);

            //ExportResultToCsv(topTenFfpUserResponse, ExportFileNameHelper.TopTenFfpPrograms);
        }

        public static string PopulateCountOfUserFfpRankOneReport(MixpanelDataExportConnector mixpanelDataExport, ReportParameter customReportParameters)
        {           
            return mixpanelDataExport.GetUserCountWithFfpAsRankOneReport(customReportParameters);

            //ExportResultToCsv(userCountWithFfpAsRankOneResponse, ExportFileNameHelper.UserWithFfpRank1);
        }

        public static string PopulateUserFfpRankReport(MixpanelDataExportConnector mixpanelDataExport, ReportParameter customReportParameters)
        {            
            return mixpanelDataExport.GetUserFfpRankReport(customReportParameters);
            //ExportResultToCsv(userFfpRankResponse, ExportFileNameHelper.UserFfpProgramPreferenceRank);
        }       
    }
}