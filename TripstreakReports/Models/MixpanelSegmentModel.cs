using System.Collections.Generic;

namespace TripstreakReports.Models
{
    public class MixpanelSegmentModel
    {
        public MixpanelSegmentModel()
        {
            Events = new List<EventModel>();
            PropertyKeys = new List<string>();
        }
        public string SessionId { get; set; }

        public ProfileModel UserMixpanelProfile { get; set; }

        public List<EventModel> Events { get; set; }

        public List<string> PropertyKeys { get; set; }
    }
}