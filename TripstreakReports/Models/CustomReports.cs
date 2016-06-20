using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripstreakReports.Models
{
    public class CustomReports
    {
        public string ReportType { get; set; }

        public string FilterProperty { get; set; }

        public string EqualTo { get; set; }

        public string Value { get; set; }

        public List<string> OutputColumns { get; set; }
    }
}