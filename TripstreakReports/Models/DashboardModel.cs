using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripstreakReports.DatAccess;

namespace TripstreakReports.Models
{
    public class DashboardModel
    {
        public bool IsProdEnvironemt { get; set; }

        public List<airamenity> Amenities { get; set; }

        public airamenity Amenity { get; set; }

        public string AmenityIds { get; set; }
    }
}