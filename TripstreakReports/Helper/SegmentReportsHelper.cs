using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmenityUpdator.UI.Helper;
using TripstreakReports.Models;

namespace TripstreakReports.Helper
{
    public static class SegmentReportsHelper
    {
        public static string GetFileName(bool isUserInfoIncluded)
        {
            return isUserInfoIncluded ? ExportFileNameHelper.SegmentReportWithUserInfo : ExportFileNameHelper.SegmentReport;
        }

        public static ReportParameter PopulateSegmentReportParameter(MixpanelModel mixpanelmodel)
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

    }
}