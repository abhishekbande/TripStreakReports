using System;
using System.Collections.Generic;

namespace TripstreakReports.Models
{
    public class ReportParameter
    {
        public List<Filter> Filter = new List<Filter>();
        public string[] OutputColumns { get; set; }
        public DateTime ReportStartDate { get; set; }
        public DateTime ReportEndDate { get; set; }

        public bool IncludeUserInformation { get; set; }
    }

    public class Filter
    {
        public string SelectedValue { get; set; }
        public string SelectedProptery { get; set; }
        public string SelectedOperator { get; set; }
    }
}
