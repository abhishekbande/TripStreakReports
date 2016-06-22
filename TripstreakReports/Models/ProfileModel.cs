using System.Collections.Generic;

namespace TripstreakReports.Models
{
   public class ProfileModel
    {
       public ProfileModel()
       {
           FlightFfpProgram = new Dictionary<string, string>();
           
           HotelFfpProgram = new Dictionary<string, string>();

           CarFfpProgram = new Dictionary<string, string>();

       }
       public Dictionary<string,string> FlightFfpProgram { get; set; }

       public Dictionary<string, string> HotelFfpProgram { get; set; }

       public Dictionary<string, string> CarFfpProgram { get; set; }

    }
}
