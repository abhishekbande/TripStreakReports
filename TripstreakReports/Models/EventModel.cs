using System.Collections.Generic;

namespace TripstreakReports.Models
{
    public class EventModel
    {
        public EventModel()
        {
            Properties = new Dictionary<string, string>();
        }

        public string EventName{get;set;}

        public Dictionary<string, string> Properties { get; set; }
    }
}
