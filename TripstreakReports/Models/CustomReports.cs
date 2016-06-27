using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripStreak.Infrastructure.Analytics.Helper;

namespace TripstreakReports.Models
{
    public class CustomReports : BaseReports
    {
        public CustomReports()
        {
            PopulateProperties();
            PopulateReports();
            PopulateFilters();
        }

        public string ReportType { get; set; }

        public string FilterProperty { get; set; }

        public string SelectedOperator { get; set; }

        public string Value { get; set; }

        public string[] OutputColumns { get; set; }        
    }
}