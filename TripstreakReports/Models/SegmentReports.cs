using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripstreakReports.Models
{
    public class SegmentReports
    {
        public string Fromdate { get; set; }

        public string ToDate { get; set; }

        public bool IncludeUserInformation { get; set; }
    }
}