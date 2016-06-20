using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace TripstreakReports.Models
{
    public class MixpanelModel
    {
        public bool IsProdEnvironemt { get; set; }

        public PreconfiguredReports PreconfiguredReports { get; set; }

        public SegmentReports SegmentReports { get; set; }

        public CustomReports CustomReports { get; set; }
     }
}