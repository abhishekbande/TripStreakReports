using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using TripstreakReports.Models;

namespace TripstreakReports.Helper
{
    public static class SegmentColumnHelper
    {
        static SegmentColumnHelper()
        {
            SegmentReportColumn = new List<SegmentColumn>();
            MandatoryReportColumn = new List<SegmentColumn>();
            Initialize();
        }

        private static void Initialize()
        {
            GetMandatoryColumns();

            GetSegmentReportColumns();

        }

        private static void GetMandatoryColumns()
        {
            //SegmentColumn column = new SegmentColumn();
            //column.ColumnName = "UserId";
            //MandatoryReportColumn.Add(column);

            //column = new SegmentColumn();
            //column.ColumnName = "EventName";
            //MandatoryReportColumn.Add(column);
            
            //column = new SegmentColumn();
            //column.ColumnName = "SessionId";
            //MandatoryReportColumn.Add(column);


            int index = 1;
            string line = string.Empty;
            string path = HttpContext.Current.Server.MapPath("~/App_Data/MandatorySegmentColumns.txt"); //"~/App_Data/SegmentReportColumns.txt";

            if (string.IsNullOrEmpty(path))
            {
                throw new ConfigurationException("The location of SegmentReportColumns file is incorrect.");
            }

            StreamReader file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                SegmentColumn column = new SegmentColumn();
                column.ColumnName = line;
                column.SequenceId = index;
                MandatoryReportColumn.Add(column);

                index++;
            }
        }

        private static void GetSegmentReportColumns()
        {
            int index = 1;
            string line = string.Empty;
            string path = HttpContext.Current.Server.MapPath("~/App_Data/SegmentReportColumns.txt"); //"~/App_Data/SegmentReportColumns.txt";

            if (string.IsNullOrEmpty(path))
            {
                throw new ConfigurationException("The location of SegmentReportColumns file is incorrect.");
            }

            StreamReader file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                SegmentColumn column = new SegmentColumn();
                column.ColumnName = line;
                column.SequenceId = index;
                SegmentReportColumn.Add(column);

                index++;
            }
        }

      public static List<SegmentColumn> SegmentReportColumn { get; set; }

      public static List<SegmentColumn> MandatoryReportColumn { get; set; }
    }
}