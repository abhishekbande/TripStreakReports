using System.Collections.Generic;
using System.IO;
using System.Linq;
using TripStreak.Infrastructure.Analytics.Constants;
using TripStreak.Infrastructure.Analytics.Entity;
using TripStreak.Infrastructure.Analytics.Implementation;
using System.Reflection;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System;
using System.Configuration;
using Newtonsoft.Json;
using AmenityUpdator.UI.Helper;
using TripstreakReports.Helper;
using TripstreakReports.Models;
using TripStreak.Infrastructure.Analytics.Helper;


namespace AmenityUpdator.UI.Controllers
{
    public class MixpanelDataExportConnector
    {
        internal string ApiKey { get; set; }

        internal string ApiSecret { get; set; }

        internal string Token { get; set; }

        protected string EventFilter
        {
            get { return ConfigurationManager.AppSettings["EventFilter"]; }
        }
        public MixpanelDataExportConnector(MixpanelKeys keys)
        {
            ApiKey = keys.ApiKey;
            ApiSecret = keys.ApiSecret;
            Token = keys.Token;
        }

        #region Preconfigured Report

        public string GetUserFfpRankReport()
        {

            MixpanelDownloadRequest mixpanelDownloadRequest= new MixpanelDownloadRequest();
            mixpanelDownloadRequest.ApiKey = ApiKey;
            mixpanelDownloadRequest.ApiSecret = ApiSecret;
            mixpanelDownloadRequest.SelectorParameters = new Dictionary<string, object>
            {
                {"selector", MixpanelSelectorConstants.PreconfigUserFfpRankReportTemplate}
            };

            MixpanelDataDownloadConnector mixpanelDataDownloadConnector = new MixpanelDataDownloadConnector();
            RootObject result = mixpanelDataDownloadConnector.DownloadProfileData(mixpanelDownloadRequest);

            return ExtractUserFfpRankData(result);
        }
        
        public string GetUserCountWithFfpAsRankOneReport()
        {

            MixpanelDownloadRequest mixpanelDownloadRequest = new MixpanelDownloadRequest();
            mixpanelDownloadRequest.ApiKey = ApiKey;
            mixpanelDownloadRequest.ApiSecret = ApiSecret;
            mixpanelDownloadRequest.SelectorParameters = new Dictionary<string, object>
            {
                {"selector", MixpanelSelectorConstants.PreconfigUserCountWithFfpAsRankOneReportTemplate}
            };

            MixpanelDataDownloadConnector mixpanelDataDownloadConnector = new MixpanelDataDownloadConnector();
            RootObject result = mixpanelDataDownloadConnector.DownloadProfileData(mixpanelDownloadRequest);
            return ExtractUserFfpRankData(result);
        }

        public string GetTopTenFfpUserPrograms()
        {
            RootObject result = DownloadProfileData();
            return ExtractTopTenProgramData(result);
        }

        private RootObject DownloadProfileData()
        {
            MixpanelDownloadRequest mixpanelDownloadRequest = new MixpanelDownloadRequest();
            mixpanelDownloadRequest.ApiKey = ApiKey;
            mixpanelDownloadRequest.ApiSecret = ApiSecret;
            mixpanelDownloadRequest.SelectorParameters = new Dictionary<string, object>
            {
                {"selector", MixpanelSelectorConstants.PreconfigTopTenFfpUserProgramsTemplate}
            };

            MixpanelDataDownloadConnector mixpanelDataDownloadConnector = new MixpanelDataDownloadConnector();
            RootObject result = mixpanelDataDownloadConnector.DownloadProfileData(mixpanelDownloadRequest);
            return result;
        }

        #endregion Preconfigured Report

        #region CustomReport

        public string GetUserFfpRankReport(ReportParameter customReportParameter)
        {
            RootObject result = null;
           
            string customSelector = MixpanelSelectorConstants.PreconfigUserFfpRankReportTemplate;

            if (customReportParameter != null && !string.IsNullOrEmpty(customReportParameter.Filter.SelectedOperator) &&
                !string.IsNullOrEmpty(customReportParameter.Filter.SelectedProptery) &&
                !string.IsNullOrEmpty(customReportParameter.Filter.SelectedValue))
            {
                customSelector = customSelector + " and " +
                                        GetCustomSelector(customReportParameter.Filter.SelectedOperator,
                                            customReportParameter.Filter.SelectedProptery,
                                            customReportParameter.Filter.SelectedValue);
            }            

            MixpanelDownloadRequest mixpanelDownloadRequest = new MixpanelDownloadRequest();
            mixpanelDownloadRequest.ApiKey = ApiKey;
            mixpanelDownloadRequest.ApiSecret = ApiSecret;
            mixpanelDownloadRequest.SelectorParameters = new Dictionary<string, object>
            {
                {"selector", customSelector}
            };
            MixpanelDataDownloadConnector mixpanelDataDownloadConnector = new MixpanelDataDownloadConnector();
            result = mixpanelDataDownloadConnector.DownloadProfileData(mixpanelDownloadRequest);
         
            return ExtractUserFfpRankData(result);
        }

        public string GetUserCountWithFfpAsRankOneReport(ReportParameter customReportParameter)
        {
            string customSelector = MixpanelSelectorConstants.PreconfigUserCountWithFfpAsRankOneReportTemplate;

            if (customReportParameter != null && !string.IsNullOrEmpty(customReportParameter.Filter.SelectedOperator) &&
                !string.IsNullOrEmpty(customReportParameter.Filter.SelectedProptery) &&
                !string.IsNullOrEmpty(customReportParameter.Filter.SelectedValue))
            {
                customSelector = customSelector + " and " +
                                        GetCustomSelector(customReportParameter.Filter.SelectedOperator, customReportParameter.Filter.SelectedProptery, customReportParameter.Filter.SelectedValue);
            }          
           
            MixpanelDownloadRequest mixpanelDownloadRequest = new MixpanelDownloadRequest();
            mixpanelDownloadRequest.ApiKey = ApiKey;
            mixpanelDownloadRequest.ApiSecret = ApiSecret;
            mixpanelDownloadRequest.SelectorParameters = new Dictionary<string, object>
            {
                {"selector", customSelector}
            };
            MixpanelDataDownloadConnector mixpanelDataDownloadConnector = new MixpanelDataDownloadConnector();
            RootObject result = mixpanelDataDownloadConnector.DownloadProfileData(mixpanelDownloadRequest);
            
            return ExtractUserFfpRankData(result);
        }

        private string GetCustomSelector(string selectedOperator,string propertyName, string value)
        {
            switch (selectedOperator)
            {
                case "equal":
                    return MixpanelSelectorConstants.IsEqualTemplate.Replace("@PropertyName", propertyName).Replace("@PropertyValue", value);
                case "notequal":
                    return MixpanelSelectorConstants.IsNotEqualTemplate.Replace("@PropertyName", propertyName).Replace("@PropertyValue", value);
                case "contains":
                    return MixpanelSelectorConstants.ContainsTemplate.Replace("@PropertyName", propertyName).Replace("@PropertyValue", value);
                case "doesnotcontain":
                    return MixpanelSelectorConstants.DoesNotContainTemplate.Replace("@PropertyName", propertyName).Replace("@PropertyValue", value);
                case "isset":
                    return MixpanelSelectorConstants.IsSetTemplate.Replace("@PropertyName", propertyName);
                case "notset":
                    return MixpanelSelectorConstants.IsNotSetTemplate.Replace("@PropertyName", propertyName);
                default:
                    return string.Empty;
            }
        }

        public string GetTopTenFfpUserPrograms(ReportParameter customReportParameter)
        {
            string customSelector = MixpanelSelectorConstants.PreconfigTopTenFfpUserProgramsTemplate;

            if (customReportParameter != null && !string.IsNullOrEmpty(customReportParameter.Filter.SelectedOperator) &&
                !string.IsNullOrEmpty(customReportParameter.Filter.SelectedProptery) &&
                !string.IsNullOrEmpty(customReportParameter.Filter.SelectedValue))
            {
                customSelector = customSelector + " and " +
                                        GetCustomSelector(customReportParameter.Filter.SelectedOperator, customReportParameter.Filter.SelectedProptery, customReportParameter.Filter.SelectedValue);
            }               

            MixpanelDownloadRequest mixpanelDownloadRequest = new MixpanelDownloadRequest();
            mixpanelDownloadRequest.ApiKey = ApiKey;
            mixpanelDownloadRequest.ApiSecret = ApiSecret;
            mixpanelDownloadRequest.SelectorParameters = new Dictionary<string, object>
            {
                {"selector", customSelector}
            };
            MixpanelDataDownloadConnector mixpanelDataDownloadConnector = new MixpanelDataDownloadConnector();
            RootObject result = mixpanelDataDownloadConnector.DownloadProfileData(mixpanelDownloadRequest);

            return ExtractTopTenProgramData(result);
        }

        public string GetAdhocReport(ReportParameter customReportParameter)
        {
            string customSelector = GetCustomSelector(customReportParameter.Filter.SelectedOperator, customReportParameter.Filter.SelectedProptery, customReportParameter.Filter.SelectedValue);

            MixpanelDownloadRequest mixpanelDownloadRequest = new MixpanelDownloadRequest();
            mixpanelDownloadRequest.ApiKey = ApiKey;
            mixpanelDownloadRequest.ApiSecret = ApiSecret;
            mixpanelDownloadRequest.SelectorParameters = new Dictionary<string, object>
            {
                {"selector", customSelector},
               
            };
                      
            MixpanelDataDownloadConnector mixpanelDataDownloadConnector = new MixpanelDataDownloadConnector();
            RootObject result = mixpanelDataDownloadConnector.DownloadProfileData(mixpanelDownloadRequest);

            return ExtractAdhocReportData(result,customReportParameter.OutputColumns);
        }

        public string GetSegmentReport(ReportParameter customReportParameter)
        {
           //string customSelector = GetCustomSelector(customReportParameter.Filter[0].SelectedOperator, 
           //    customReportParameter.Filter[0].SelectedProptery, 
           //    customReportParameter.Filter[0].SelectedValue);

            SetDatesToDefaultIfNull(customReportParameter);

            MixpanelDownloadRequest mixpanelDownloadRequest = new MixpanelDownloadRequest();
            mixpanelDownloadRequest.ApiKey = ApiKey;
            mixpanelDownloadRequest.ApiSecret = ApiSecret;
            string[] eventFilters = EventFilter.Split(',');

            mixpanelDownloadRequest.SelectorParameters = new Dictionary<string, object>
            {
                //{"where", customSelector},
                //{"event",EventFilter},
                {"from_date",customReportParameter.ReportStartDate.Date.ToString("yyyy-MM-dd")},// "2016-04-15"},
                {"to_date",customReportParameter.ReportEndDate.Date.ToString("yyyy-MM-dd")},//"2016-04-15"}
            };
            MixpanelDataDownloadConnector mixpanelDataDownloadConnector = new MixpanelDataDownloadConnector();
            RootObject result = mixpanelDataDownloadConnector.DownloadRawData(mixpanelDownloadRequest);
            RootObject profileData = DownloadProfileData();

            List<MixpanelSegmentModel> segmentModels = MapResultToModel(result, profileData);
            
            List<string> columns = new List<string>();
            segmentModels.ForEach(s => columns.AddRange(s.PropertyKeys));
            columns = columns.Distinct().ToList();

            FilterColumns(columns, customReportParameter, GetUserInfoColumnsList(),GetColumnsToExclude());

            return ExtractMasterReportData(segmentModels, columns);
            
        }

        private List<string> GetColumnsToExclude()
        {
            List<string> excludeColumns = new List<string>();
            string[] extraColumns = ConfigurationManager.AppSettings["MixpanelColumnExclusion"].Split(',');
            excludeColumns.AddRange(extraColumns);
            return excludeColumns;
        }

        private List<string> GetUserInfoColumnsList()
        {
            List<string> userInfoColumns = new List<string>();
            string[] columns = ConfigurationManager.AppSettings["MixpanelUserInfoColumns"].Split(',');
            userInfoColumns.AddRange(columns);

           
            return userInfoColumns;
        }

        private void FilterColumns(List<string> columns, ReportParameter customReportParameter, List<string> userInfoColumns, List<string> columnsToExclude)
        {
            if (!customReportParameter.IncludeUserInformation)
            {
                foreach (string column in userInfoColumns)
                {
                    if(columns.Contains(column))
                    {
                        columns.Remove(column);
                    }
                }
            }

            foreach (string col in columnsToExclude)
            {
                if (columns.Contains(col))
                {
                    columns.Remove(col);
                }
            }

        }

        private static void SetDatesToDefaultIfNull(ReportParameter customReportParameter)
        {
            if (customReportParameter.ReportStartDate == null || customReportParameter.ReportStartDate == DateTime.MinValue)
            {
                customReportParameter.ReportStartDate = DateTime.Today.Date.AddDays(-30);
            }

            if (customReportParameter.ReportEndDate == null || customReportParameter.ReportEndDate == DateTime.MinValue)
            {
                customReportParameter.ReportEndDate = DateTime.Today.Date;
            }
        }

        private string ExtractMasterReportData(List<MixpanelSegmentModel> segmentModels, List<string> columns)
        {

            DataTable segmentTable = new DataTable();
            GetDataTableColumns(segmentTable, columns);

            foreach (var model in segmentModels)
            {
                DataRow segmentRow = segmentTable.NewRow();
                PopulateSegmentReportRow(columns, segmentRow, model);
                segmentTable.Rows.Add(segmentRow);
            }
            
            return GenerateResponse(segmentTable);
        }

        private static string GenerateResponse(DataTable segmentTable)
        {
            if (segmentTable == null || segmentTable.Rows == null || segmentTable.Rows.Count == 0)
                return "No data to display";

            segmentTable = segmentTable.AsEnumerable().OrderByDescending(r => r.Field<string>("time")).ThenBy(r => r.Field<string>("SessionId")).CopyToDataTable();

            StringBuilder sb = new StringBuilder();

            var columnNames = segmentTable.Columns.Cast<DataColumn>().Select(column => "\"" + column.ColumnName.Replace("\"", "\"\"") + "\"").ToArray();
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in segmentTable.Rows)
            {
                var fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").ToArray();
                sb.AppendLine(string.Join(",", fields));
            }

            return sb.ToString();
        }

        private DataRow PopulateSegmentReportRow(List<string> columns, DataRow segmentRow, MixpanelSegmentModel model)
        {

            foreach (var eventDetail in model.Events)
            {
                segmentRow["SessionId"] = model.SessionId;
                segmentRow["EventName"] = segmentRow["EventName"] == null ||
                    string.IsNullOrEmpty(segmentRow["EventName"].ToString()) ? eventDetail.EventName : segmentRow["EventName"].ToString() + ", " + eventDetail.EventName;

                GetColumnwiseSegmentData(columns, segmentRow, eventDetail);

               

                if (string.IsNullOrEmpty(segmentRow["UserId"].ToString()))
                {
                    segmentRow["UserId"] = model.SessionId;
                }
            }

            if (segmentRow["UserId"].ToString().Length < 20)
            {
                GetProfileDetails(segmentRow, model.UserMixpanelProfile);
            }

            return segmentRow;
        }

        private void GetProfileDetails(DataRow segmentRow, ProfileModel profileModel)
        {
            if (profileModel == null)
                return;

            GetFlightFfpProgramDetails(segmentRow, profileModel);

            GetHotelFfpProgramDetails(segmentRow, profileModel);

            GetCarFfpProgramDetails(segmentRow, profileModel);
        }

        private static void GetCarFfpProgramDetails(DataRow segmentRow, ProfileModel profileModel)
        {
            foreach (var keyValue in profileModel.CarFfpProgram)
            {
                segmentRow[keyValue.Key] = keyValue.Value;
            }
        }

        private static void GetHotelFfpProgramDetails(DataRow segmentRow, ProfileModel profileModel)
        {
            foreach (var keyValue in profileModel.HotelFfpProgram)
            {
                segmentRow[keyValue.Key] = keyValue.Value;
            }
        }

        private static void GetFlightFfpProgramDetails(DataRow segmentRow, ProfileModel profileModel)
        {
            foreach (var keyValue in profileModel.FlightFfpProgram)
            {
                segmentRow[keyValue.Key] = keyValue.Value;
            }
        }

        private void GetColumnwiseSegmentData(List<string> columns, DataRow segmentRow, EventModel eventDetail)
        {
            foreach (var column in columns)
            {
                try
                {
                    string value = GetPropertyValue(eventDetail, column);
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (column == "time")
                        {
                            long seconds = long.Parse(value);
                            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

                            epoch = epoch.AddSeconds(seconds);
                            value = epoch.ToString();

                        }

                        if (column.ToLower() == "distinct_id" || column.ToLower() == "user_id" || column.ToLower() == "id")
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                segmentRow["UserId"] = value;
                            }
                            continue;
                        }
                        segmentRow[column] = value;
                    }
                }
                catch
                {
                }
            }
        }

        private static void GetDataTableColumns(DataTable segmentTable, List<string> columns)
        {
            //GetMandatoryColumns(segmentTable);

            GetConfiguredSegmentReportColumns(segmentTable, columns);

            GetAdditionalColumns(segmentTable, columns);
                
        }

        private static void GetAdditionalColumns(DataTable segmentTable, List<string> columns)
        {
            foreach (var column in columns)
            {
                if (!segmentTable.Columns.Contains(column) && (column.ToLower() != "distinct_id" && column.ToLower() != "sessionid" && column.ToLower() != "user_id"))
                {
                    segmentTable.Columns.Add(column, typeof(string));
                }
            }
        }

        private static void GetConfiguredSegmentReportColumns(DataTable segmentTable, List<string> columns)
        {
            foreach (var segmentColumns in SegmentColumnHelper.SegmentReportColumn.OrderBy(s => s.SequenceId))
            {
                if (IsMandatoryColumn(segmentColumns.ColumnName.ToLower()) || (columns.Find(c => c.ToLower() == segmentColumns.ColumnName.ToLower()) != null && 
                    !segmentTable.Columns.Contains(segmentColumns.ColumnName) &&
                    (segmentColumns.ColumnName.ToLower() != "distinct_id" && segmentColumns.ColumnName.ToLower() != "sessionid" && segmentColumns.ColumnName.ToLower() != "user_id")))
                {
                    segmentTable.Columns.Add(segmentColumns.ColumnName, typeof(string));
                }
            }
        }

        private static bool IsMandatoryColumn(string column)
        {
            var mandatoryColumn = SegmentColumnHelper.MandatoryReportColumn.Find(col => col.ColumnName.ToLower() == column);
            return mandatoryColumn != null ? true : false;
        }

        private static void GetMandatoryColumns(DataTable segmentTable)
        {
            segmentTable.Columns.Add("UserId", typeof(string));
            segmentTable.Columns.Add("EventName", typeof(string));
            segmentTable.Columns.Add("SessionId", typeof(string));
        }

        private string GetPropertyValue(EventModel eventDetail, string column)
        {
            string propertyValue;
            eventDetail.Properties.TryGetValue(column, out propertyValue);
            return string.IsNullOrEmpty(propertyValue) ? string.Empty : propertyValue;
        }

        #endregion

        #region GetReportDataFromMixpanel


        private static string ExtractUserFfpRankData(RootObject mixpanelData)
        {
            if (mixpanelData == null || mixpanelData.Results == null)
                return string.Empty;

            StringWriter stringWriter = new StringWriter();
            stringWriter.WriteLine("\"UserId\",\"Email\",\"Name\",\"City\",\"Country\",\"FFP Program Rank\"");

            foreach (var line in mixpanelData.Results.OrderBy(r=>r.Properties.FlightFrequentFlierProgramPreferenceRank))
            {
                stringWriter.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"", 
                                                     line.Properties.UserId,
                                                     line.Properties.Email,
                                                     (line.Properties.FirstName+" " + line.Properties.LastName),
                                                     line.Properties.City,
                                                     line.Properties.CountryCode,
                                                     line.Properties.FlightFrequentFlierProgramPreferenceRank));
            }
            return stringWriter.ToString();
        }

        private string ExtractTopTenProgramData(RootObject topTenFfpUserProgram)
        {
            if (topTenFfpUserProgram == null || topTenFfpUserProgram.Results == null)
                return string.Empty;

            StringWriter stringWriter = new StringWriter();
            stringWriter.WriteLine("\"UserId\",\"FFP Program\"");

            foreach (var line in topTenFfpUserProgram.Results.OrderBy(r=>r.Properties.FlightFfpPreference1))
            {
                stringWriter.WriteLine("\"{0}\",\"{1}\"", line.Properties.UserId, line.Properties.FlightFfpPreference1);
            }
            return stringWriter.ToString();
        }

        private string ExtractMasterReportData(RootObject adhocResponse, string[] outputColumns, List<SelectListItem> properties)
        {

            if (adhocResponse == null || adhocResponse.SegmentData == null || adhocResponse.SegmentData.Count==0)
                return string.Empty;
                       

            string columnTemplate = "\"@ColumnName\"";

            StringWriter stringWriter = new StringWriter();
            stringWriter.Write(columnTemplate.Replace("@ColumnName", "SessionId"));
            stringWriter.Write(",");

            StringWriter rowWriter = new StringWriter();


            //foreach (SelectListItem column in properties)
            //{
            //    stringWriter.Write(columnTemplate.Replace("@ColumnName", column.Text));
            //    stringWriter.Write(",");
            //}
            foreach (var column in outputColumns)
            {
                SelectListItem property = properties.Find(p => p.Value == column);
                if (property != null && (property.Text!="SessionId" || property.Value!="SessionId"))
                {
                    stringWriter.Write(columnTemplate.Replace("@ColumnName",property!=null? property.Text: column));
                    stringWriter.Write(",");
                }
            }
            bool isColumnAvailable = false;
            List<string> columnNames =new List<string>();

           

           //adhocResponse.SegmentData = adhocResponse.SegmentData.GroupBy(r=>r.DistinctId).ToList<Segments>();

            foreach (var result in adhocResponse.SegmentData.OrderBy(d => d.SessionId).ThenBy(d=>d.EventName))
            {
                rowWriter.Write(result.EventName);
                rowWriter.Write(",");

                //foreach (SelectListItem column in properties)
                foreach (var column in outputColumns)
                {
                    string value = GetPropertyValue(result, column);
                    //if (!string.IsNullOrEmpty(value))
                    {
                        //if (columnNames.Count == 0 || !columnNames.Contains(column.Text))
                        {
                            //columnNames.Add(column.Text);
                            //stringWriter.Write(columnTemplate.Replace("@ColumnName", column.Text));
                            //stringWriter.Write(",");
                        }

                        rowWriter.Write(value);
                        rowWriter.Write(",");
                    }
                }
                isColumnAvailable = true;
                rowWriter.WriteLine();
            }
            stringWriter.WriteLine();
            stringWriter.Write(rowWriter.ToString());
            return stringWriter.ToString();
        }

        private List<MixpanelSegmentModel> MapResultToModel(RootObject adhocResponse,RootObject profileData)
        {
            List<MixpanelSegmentModel> segmentModels = new List<MixpanelSegmentModel>();

            segmentModels = ClubEventsWithSameSessionId(adhocResponse, profileData);

            segmentModels.AddRange(GetIndependentSessionEvents(adhocResponse, profileData));

            return segmentModels;

        }

        private IEnumerable<MixpanelSegmentModel> GetIndependentSessionEvents(RootObject adhocResponse, RootObject profileData)
        {
            List<MixpanelSegmentModel> segmentModels = new List<MixpanelSegmentModel>();
            IEnumerable<Segments> independentSegments = adhocResponse.SegmentData.Where(s => string.IsNullOrEmpty(s.SessionId)).Select(s => s);
            foreach (Segments segment in independentSegments)
            {
                MixpanelSegmentModel segmentModel = new MixpanelSegmentModel();
                segmentModel.SessionId = segment.SessionId;
                segmentModel.Events = GetAllEventForSession(segment, segmentModel.PropertyKeys);
                segmentModel.UserMixpanelProfile = GetMixpanelProfileData(GetUserId(segmentModel.Events), profileData, segmentModel.PropertyKeys);
                segmentModels.Add(segmentModel);
            }
            return segmentModels;
        }

        private List<EventModel> GetAllEventForSession(Segments segment, List<string> uniqueKeys)
        {
            List<EventModel> events = new List<EventModel>();
            EventModel eventModel = new EventModel();
            eventModel.EventName = segment.EventName;
            eventModel.Properties = GetPropertyData(segment.Properties, uniqueKeys);
            events.Add(eventModel);
            return events;
        }

        private List<MixpanelSegmentModel> ClubEventsWithSameSessionId(RootObject adhocResponse, RootObject profileData)
        {
            List<MixpanelSegmentModel> segmentModels = new List<MixpanelSegmentModel>();
            List<string> uniqueSessions = adhocResponse.SegmentData.Where(s => !string.IsNullOrEmpty(s.SessionId)).Select(s => s.SessionId).Distinct().ToList();
            foreach (string sessionId in uniqueSessions)
            {
                MixpanelSegmentModel segmentModel = new MixpanelSegmentModel();
                segmentModel.SessionId = sessionId;
                segmentModel.Events = GetAllEventForSession(adhocResponse.SegmentData.Where(s => s.SessionId == sessionId), segmentModel.PropertyKeys);
                segmentModel.UserMixpanelProfile = GetMixpanelProfileData(GetUserId(segmentModel.Events), profileData, segmentModel.PropertyKeys);
                segmentModels.Add(segmentModel);
            }
            return segmentModels;
        }

        private ProfileModel GetMixpanelProfileData(string userId, RootObject profileData, List<string> uniqueKeys)
        {

            ProfileModel profileModel = new ProfileModel();

            var userProfile = profileData.Results.Find(r => r.DistinctId == userId);
            if (userProfile != null)
            {
                GetFlightFfpPrograms(profileModel, userProfile);

                GetHotelFfpDetails(profileModel, userProfile);

                GetCarFfpDetails(profileModel, userProfile);
            }

            AddFfpKeysinList(uniqueKeys);
            return profileModel;
        }

        private void AddFfpKeysinList(List<string> uniqueKeys)
        {
            uniqueKeys.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram1);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram2);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram3);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram4);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram5);

            uniqueKeys.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram1);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram2);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram3);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram4);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram5);

            uniqueKeys.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram1);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram2);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram3);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram4);
            uniqueKeys.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram5);
        }

        private static void GetCarFfpDetails(ProfileModel profileModel, Result userProfile)
        {
            profileModel.CarFfpProgram.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram1, userProfile.Properties.CarFfpPreference1);
            profileModel.CarFfpProgram.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram2, userProfile.Properties.CarFfpPreference2);
            profileModel.CarFfpProgram.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram3, userProfile.Properties.CarFfpPreference3);
            profileModel.CarFfpProgram.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram4, userProfile.Properties.CarFfpPreference4);
            profileModel.CarFfpProgram.Add(MixpanelPropertyKeyHelper.CarFfpPreferenceProgram5, userProfile.Properties.CarFfpPreference5);
        }

        private static void GetHotelFfpDetails(ProfileModel profileModel, Result userProfile)
        {
            profileModel.HotelFfpProgram.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram1, userProfile.Properties.HotelFfpPreference1);
            profileModel.HotelFfpProgram.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram2, userProfile.Properties.HotelFfpPreference2);
            profileModel.HotelFfpProgram.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram3, userProfile.Properties.HotelFfpPreference3);
            profileModel.HotelFfpProgram.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram4, userProfile.Properties.HotelFfpPreference4);
            profileModel.HotelFfpProgram.Add(MixpanelPropertyKeyHelper.HotelFfpPreferenceProgram5, userProfile.Properties.HotelFfpPreference5);
        }

        private static void GetFlightFfpPrograms(ProfileModel profileModel, Result userProfile)
        {
            profileModel.FlightFfpProgram.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram1, userProfile.Properties.FlightFfpPreference1);
            profileModel.FlightFfpProgram.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram2, userProfile.Properties.FlightFfpPreference2);
            profileModel.FlightFfpProgram.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram3, userProfile.Properties.FlightFfpPreference3);
            profileModel.FlightFfpProgram.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram4, userProfile.Properties.FlightFfpPreference4);
            profileModel.FlightFfpProgram.Add(MixpanelPropertyKeyHelper.AirlineOrFfpPreferenceProgram5, userProfile.Properties.FlightFfpPreference5);
        }

        private string GetUserId(List<EventModel> events)
        {
            if (events == null || events.Count == 0)
                return string.Empty;
            try
            {
                 string userId =string.Empty;

                var eventWithUserId = events.Find(e => e.Properties.Any(p => p.Key == "distinct_id" && !string.IsNullOrEmpty(p.Value)));
                if (eventWithUserId != null)
                {
                    userId = eventWithUserId.Properties["distinct_id"].ToString();
                }
                return userId;
            }
            catch
            {
                return string.Empty;
            }
        }

        private List<EventModel> GetAllEventForSession(IEnumerable<Segments> segments, List<string> uniqueKeys)
        {
             List<EventModel> eventModels = new List<EventModel>();
            foreach (Segments segment in segments)
            {
                EventModel eventModel = new EventModel();
                eventModel.EventName = segment.EventName;
                eventModel.Properties = GetPropertyData(segment.Properties, uniqueKeys);
                eventModels.Add(eventModel);
            }
            return eventModels;
        }

        private Dictionary<string, string> GetPropertyData(JObject jObject, List<string> uniqueKeys)
        {
            Dictionary<string,string> properties= new Dictionary<string, string>();
            foreach (var property in jObject)
            {
                string key = string.Empty;
                string value=string.Empty;
                try
                {
                    key = property.Key;
                    uniqueKeys.Add(key);
                    JValue jValue = property.Value as JValue;
                    if (jValue != null)
                    {
                        if (jValue.Value != null)
                        {
                            value = jValue.Value.ToString();
                        }
                        properties.Add(key, value);
                    }
                    else
                    {

                    }
                }
                catch
                {
                    continue;
                }
            }
          
            return properties;
        }

        private static string GetPropertyValue(Segments result, SelectListItem column)
        {
            object value = result.Properties[column.Value];
            if (value != null)
            {
                return value.ToString();
            }
            return string.Empty;
        }

        private static string GetPropertyValue(Segments result, string columnName)
        {
            object value = result.Properties[columnName];
            if (value != null)
            {
                return value.ToString();
            }
            return string.Empty;
        }

        private string GetPropertyValue(Segments result, string column, string value)
        {
            PropertyInfo[] props = result.Properties.GetType().GetProperties();
            foreach (PropertyInfo property in props)
            {
                foreach (CustomAttributeData att in property.CustomAttributes)
                {
                    foreach (CustomAttributeNamedArgument arg in att.NamedArguments)
                    {
                        if (arg.TypedValue.Value.ToString().Contains(column))
                        {
                            return (string)property.GetValue(result.Properties, null);
                        }
                    }
                }
            }
            return value;
        }

        private string ExtractAdhocReportData(RootObject adhocResponse, string[] outputColumns)
        {

            if (adhocResponse == null || adhocResponse.Results == null)
                return string.Empty;

            if (outputColumns == null)
            {
                outputColumns = new CustomReports().Properties.Select(x=>x.Text).ToArray();
            }

            StringWriter stringWriter = new StringWriter();
            string columnTemplate = "\"@ColumnName\"";

            foreach (var column in outputColumns)
            {
                stringWriter.Write(columnTemplate.Replace("@ColumnName", column));
                stringWriter.Write(",");
            }

            foreach (var result in adhocResponse.Results)
            {
                stringWriter.WriteLine();

                foreach (string column in outputColumns)
                {
                    string value = string.Empty;
                    value = GetPropertyValue(result, column, value);
                    stringWriter.Write(value);
                    stringWriter.Write(",");
                }
            }
            return stringWriter.ToString();
        }

        private static string GetPropertyValue(Result result, string column, string value)
        {

            PropertyInfo[] props = result.Properties.GetType().GetProperties();
            foreach (PropertyInfo property in props)
            {
                foreach (CustomAttributeData att in property.CustomAttributes)
                {
                    foreach (CustomAttributeNamedArgument arg in att.NamedArguments)
                    {
                        if (arg.TypedValue.Value.ToString().Contains(column))
                        {
                            return (string)property.GetValue(result.Properties, null);
                        }
                    }
                }
            }
            return value;
        }
        
        #endregion GetReportDataFromMixpanel
    }
}