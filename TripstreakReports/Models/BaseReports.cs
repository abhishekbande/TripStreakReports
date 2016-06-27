using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TripStreak.Infrastructure.Analytics.Entity;
using TripStreak.Infrastructure.Analytics.Helper;

namespace TripstreakReports.Models
{
    public class BaseReports
    {
        public List<SelectListItem> Properties = new List<SelectListItem>();

        public List<SelectListItem> Reports = new List<SelectListItem>();

        public List<SelectListItem> OperatorItems = new List<SelectListItem>();

        protected void PopulateProperties()
        {
            //Properties.Add(new SelectListItem() { Text = "-- Select report type --", Value = string.Empty, Selected = true });

            GetUserListItems();

            GetCarPreferencesListItem();

            GetFlightPreferenceListItems();

            GetHotelPreferenceListItems();

            GetFlightSearchListItems();

            GetHotelSearchListItems();

            GetCarSearchListItems();

            GetEventProperties();
        }

        protected void PopulateFilters()
        {
            //OperatorItems.Add(new SelectListItem() { Text = "--Please select operator--", Value = string.Empty, Selected = false });
            OperatorItems.Add(new SelectListItem() { Text = "Equal to", Value = "equal", Selected = false });
            OperatorItems.Add(new SelectListItem() { Text = "Not Equal to", Value = "notequal", Selected = false });
            OperatorItems.Add(new SelectListItem() { Text = "Contains", Value = "contains", Selected = false });
            OperatorItems.Add(new SelectListItem() { Text = "Does not Contains", Value = "doesnotcontain", Selected = false });
            OperatorItems.Add(new SelectListItem() { Text = "Is Set", Value = "isset", Selected = false });
            OperatorItems.Add(new SelectListItem() { Text = "Is Not Set", Value = "notset", Selected = false });
        }

        protected void PopulateReports()
        {
            //Reports.Add(new SelectListItem() { Text = "-- Select filter property --", Value = string.Empty, Selected = true });

            Reports.Add(new SelectListItem() { Text = "Adhoc report", Value = "Adhoc", Selected = false });
            //Reports.Add(new SelectListItem() { Text = "Segment report", Value = "MasterReport", Selected = false });
            Reports.Add(new SelectListItem() { Text = "User FFP Program Preference Rank", Value = "UserFfpRanks", Selected = false });
            Reports.Add(new SelectListItem() { Text = "Number of user actually input FFP/rank miles as #1", Value = "UserCountWithFfpRank1", Selected = false });
            Reports.Add(new SelectListItem() { Text = "Top 10 FFP Programs of all users", Value = "Top10FfpPrograms", Selected = false });
        }

        protected void GetUserListItems()
        {
            Properties.Add(new SelectListItem() { Text = "UserId", Value = "user_id", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Email", Value = "email", Selected = false });
            Properties.Add(new SelectListItem() { Text = "FirstName", Value = "first_name", Selected = false });
            Properties.Add(new SelectListItem() { Text = "LastName", Value = "last_name", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Region", Value = "region", Selected = false });
            Properties.Add(new SelectListItem() { Text = "City", Value = "City", Selected = false });
            Properties.Add(new SelectListItem() { Text = "CountryCode", Value = "CountryCode", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Timezone", Value = "timezone", Selected = false });
            Properties.Add(new SelectListItem() { Text = "LastSeen", Value = "last_seen", Selected = false });
        }

        protected void GetCarPreferencesListItem()
        {
            Properties.Add(new SelectListItem()
            {
                Text = "CarAmenitiesPreferenceRank",
                Value = MixpanelPropertyKeyHelper.CarAmenitiesPreferenceRank,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "CarClassesPrefernceRank",
                Value = MixpanelPropertyKeyHelper.CarClassesPrefernceRank,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "CarDiscountPreferenceRank",
                Value = MixpanelPropertyKeyHelper.CarDiscountPreferenceRank,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "CarFareRangePreferenceRank",
                Value = MixpanelPropertyKeyHelper.CarFareRangePreferenceRank,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "CarLocationPreferenceRank",
                Value = MixpanelPropertyKeyHelper.CarLocationPreferenceRank,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "CarRentalCompaniesPreferenceRank",
                Value = MixpanelPropertyKeyHelper.CarRentalCompaniesPreferenceRank,
                Selected = false
            });
        }

        protected void GetFlightPreferenceListItems()
        {
            Properties.Add(new SelectListItem() { Text = "AirlineOrFfpPreferenceRank", Value = MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "FlightAirportLoungePreferenceRank", Value = MixpanelPropertyKeyHelper.AirportLoungePreferenceRank, Selected = false });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightAirFareRangePreferenceRank",
                Value = MixpanelPropertyKeyHelper.AirFareRangePreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightAircraftPreferenceRank",
                Value = MixpanelPropertyKeyHelper.AircraftPreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightAirlineAlliancePreferenceRank",
                Value = MixpanelPropertyKeyHelper.AirlineAlliancePreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightAirportPreferenceRank",
                Value = MixpanelPropertyKeyHelper.AirportPreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightClassOfServicePreferenceRank",
                Value = MixpanelPropertyKeyHelper.ClassOfServicePreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightCodeSharePreferenceRank",
                Value = MixpanelPropertyKeyHelper.FlightCodeSharePreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightConnectionPreferenceRank",
                Value = MixpanelPropertyKeyHelper.FlightConnectionPreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightFlyingTimePreferenceRank",
                Value = MixpanelPropertyKeyHelper.FlightFlyingTimePreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightFrequentFlierProgramPreferenceRank",
                Value = MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightLayoverRangePreferenceRank",
                Value = MixpanelPropertyKeyHelper.LayoverRangePreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightLuggageRangePreferenceRank",
                Value = MixpanelPropertyKeyHelper.LuggageRangePreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightMealTypePreferenceRank",
                Value = MixpanelPropertyKeyHelper.MealTypePreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightMilesEarningPriorityPreferenceRank",
                Value = MixpanelPropertyKeyHelper.MilesEarningPriorityPreferenceRank,
                Selected = false
            });

            Properties.Add(new SelectListItem()
            {
                Text = "FlightRedeyesPreferenceRank",
                Value = MixpanelPropertyKeyHelper.RedeyesPreferenceRank,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "FlightRefundableFlightPreferenceRank",
                Value = MixpanelPropertyKeyHelper.RefundableFlightPreferenceRank,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "AirlineOrFfpPreferenceProgram1",
                Value = MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram1,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "FlightFfpPreference2",
                Value = MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram2,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "FlightFfpPreference3",
                Value = MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram3,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "FlightFfpPreference4",
                Value = MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram4,
                Selected = false
            });
            Properties.Add(new SelectListItem()
            {
                Text = "FlightFfpPreference5",
                Value = MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram5,
                Selected = false
            });
        }

        protected void GetHotelPreferenceListItems()
        {
            Properties.Add(new SelectListItem() { Text = "HotelAirlinePreferenceRank", Value = MixpanelPropertyKeyHelper.HotelAirlinePreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelAmenitiesPreferenceRank", Value = MixpanelPropertyKeyHelper.HotelAmenitiesPreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelBedTypePreferenceRank", Value = MixpanelPropertyKeyHelper.HotelBedTypePreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelCreditCardsPreferenceRank", Value = MixpanelPropertyKeyHelper.HotelCreditCardsPreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelFareRangePreferenceRank", Value = MixpanelPropertyKeyHelper.HotelFareRangePreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelProgramPreferenceRank", Value = MixpanelPropertyKeyHelper.HotelProgramPreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelProximityPreferenceRank", Value = MixpanelPropertyKeyHelper.HotelProximityPreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelRoomRateFlexibilityPreferenceRank", Value = MixpanelPropertyKeyHelper.HotelRoomRateFlexibilityPreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSpecialRatesPreferenceRank", Value = MixpanelPropertyKeyHelper.HotelSpecialRatesPreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelStarRangePreferenceRank", Value = MixpanelPropertyKeyHelper.HotelStarRangePreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelStarRatingRangePreferenceRank", Value = MixpanelPropertyKeyHelper.HotelStarRatingRangePreferenceRank, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelUserReviewRangePreferenceRank", Value = MixpanelPropertyKeyHelper.HotelUserReviewRangePreferenceRank, Selected = false });
        }

        protected void GetFlightSearchListItems()
        {
            Properties.Add(new SelectListItem() { Text = "FlightSearch_Class", Value = MixpanelPropertyKeyHelper.FightSearch.Class, Selected = false });
            Properties.Add(new SelectListItem() { Text = "FlightSearch_Destination", Value = MixpanelPropertyKeyHelper.FightSearch.Destination, Selected = false });
            Properties.Add(new SelectListItem() { Text = "FlightSearch_FromDate", Value = MixpanelPropertyKeyHelper.FightSearch.FromDate, Selected = false });
            Properties.Add(new SelectListItem() { Text = "FlightSearch_Source", Value = MixpanelPropertyKeyHelper.FightSearch.Source, Selected = false });
            Properties.Add(new SelectListItem() { Text = "FlightSearch_ToDate", Value = MixpanelPropertyKeyHelper.FightSearch.ToDate, Selected = false });
            Properties.Add(new SelectListItem() { Text = "FlightSearch_TripType", Value = MixpanelPropertyKeyHelper.FightSearch.TripType, Selected = false });

        }

        protected void GetHotelSearchListItems()
        {
            Properties.Add(new SelectListItem() { Text = "HotelSearch_CheckInDate", Value = MixpanelPropertyKeyHelper.HotelSearch.CheckInDate, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSearch_CheckOutDate", Value = MixpanelPropertyKeyHelper.HotelSearch.CheckOutDate, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSearch_CityCode", Value = MixpanelPropertyKeyHelper.HotelSearch.CityCode, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSearch_CityName", Value = MixpanelPropertyKeyHelper.HotelSearch.CityName, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSearch_CountryCode", Value = MixpanelPropertyKeyHelper.HotelSearch.CountryCode, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSearch_CountryName", Value = MixpanelPropertyKeyHelper.HotelSearch.CountryName, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSearch_DestinationName", Value = MixpanelPropertyKeyHelper.HotelSearch.DestinationName, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSearch_TotalGuests", Value = MixpanelPropertyKeyHelper.HotelSearch.TotalGuests, Selected = false });
            Properties.Add(new SelectListItem() { Text = "HotelSearch_TotalRooms", Value = MixpanelPropertyKeyHelper.HotelSearch.TotalRooms, Selected = false });
        }

        protected void GetCarSearchListItems()
        {
            Properties.Add(new SelectListItem() { Text = "CarSearch_DropoffDate", Value = MixpanelPropertyKeyHelper.CarSearch.DropoffDate, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_DropoffLocationCityName", Value = MixpanelPropertyKeyHelper.CarSearch.DropoffLocationCityName, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_DropoffLocationCode", Value = MixpanelPropertyKeyHelper.CarSearch.DropoffLocationCode, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_DropoffLocationCountryCode", Value = MixpanelPropertyKeyHelper.CarSearch.DropoffLocationCountryCode, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_DropoffLocationCountryName", Value = MixpanelPropertyKeyHelper.CarSearch.DropoffLocationCountryName, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_DropoffLocationName", Value = MixpanelPropertyKeyHelper.CarSearch.DropoffLocationName, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_PickupDate", Value = MixpanelPropertyKeyHelper.CarSearch.PickupDate, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_PickupLocationCityName", Value = MixpanelPropertyKeyHelper.CarSearch.PickupLocationCityName, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_PickupLocationCode", Value = MixpanelPropertyKeyHelper.CarSearch.PickupLocationCode, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_PickupLocationCountryCode", Value = MixpanelPropertyKeyHelper.CarSearch.PickupLocationCountryCode, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_PickupLocationCountryName", Value = MixpanelPropertyKeyHelper.CarSearch.PickupLocationCountryName, Selected = false });
            Properties.Add(new SelectListItem() { Text = "CarSearch_PickupLocationName", Value = MixpanelPropertyKeyHelper.CarSearch.PickupLocationName, Selected = false });

        }

        protected void GetEventProperties()
        {
            GetPricingEventProperties();

            GetBookingEventProperties();

            Properties.Add(new SelectListItem() { Text = "Status", Value = "status", Selected = false });
            Properties.Add(new SelectListItem() { Text = "SessionId", Value = "sessionId", Selected = false });
        }

        private void GetPricingEventProperties()
        {
            Properties.Add(new SelectListItem() { Text = "Pricing_Amount", Value = "amount", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Pricing_Currency", Value = "currency", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Pricing_Farechange", Value = "fare_change", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Pricing_ProductType", Value = "product_type", Selected = false });
            //Properties.Add(new SelectListItem() { Text = "Pricing_status", Value = "status", Selected = false });
        }

        private void GetBookingEventProperties()
        {
            Properties.Add(new SelectListItem() { Text = "Booking_Amount", Value = "amount", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Booking_currency", Value = "currency", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Booking_FFP", Value = "ffp", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Booking_ProductType", Value = "product_type", Selected = false });
            Properties.Add(new SelectListItem() { Text = "Booking_TripId", Value = "trip_id", Selected = false });
            //Properties.Add(new SelectListItem() { Text = "Booking_status", Value = "status", Selected = false });
        }       
    }
}
