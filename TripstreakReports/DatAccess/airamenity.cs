//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TripstreakReports.DatAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class airamenity
    {
        public int ID { get; set; }
        public string Airline { get; set; }
        public string AirlineCode { get; set; }
        public string Manufacturer { get; set; }
        public string AircraftModel { get; set; }
        public string AircraftType { get; set; }
        public string IataTypeMapping { get; set; }
        public string Cabin { get; set; }
        public string VideoType { get; set; }
        public string InSeatPowerType { get; set; }
        public string WiFiAvailablilty { get; set; }
        public string SeatType { get; set; }
        public string AvailableAtArrivalAirport { get; set; }
        public string AvailableAtDepartureAirport { get; set; }
        public string AvailableInCountry { get; set; }
        public string BeingUpgraded { get; set; }
        public Nullable<System.DateTime> LastUpdatedOn { get; set; }
    }
}
