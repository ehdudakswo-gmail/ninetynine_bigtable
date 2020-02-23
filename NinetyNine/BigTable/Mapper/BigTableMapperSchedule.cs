using NinetyNine.Template;
using System;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperSchedule : BigTableMapper
    {
        internal BigTableMapperSchedule(DataTable bigTable) : base(bigTable)
        {
        }

        internal override void Mapping()
        {
            throw new System.NotImplementedException();
        }

        protected DateTime GetDateTime(DataRow scheduleRow, ScheduleTitle title)
        {
            int colIdx = GetColumnIdx(title);
            string dateStr = scheduleRow[colIdx].ToString();
            DateTime dateTime = DateTime.Parse(dateStr);

            return dateTime;
        }

        protected string GetYear(DateTime dateTime)
        {
            string year = dateTime.ToString("yyyy");

            return year;
        }

        protected string GetQuarter(DateTime dateTime)
        {
            string monthStr = dateTime.ToString("MM");
            int monthNum;
            int.TryParse(monthStr, out monthNum);

            int quarterNum = ((monthNum - 1) / 3) + 1;
            string quarterStr = quarterNum.ToString();

            return quarterStr;
        }

        protected string GetMonth(DateTime dateTime)
        {
            string month = dateTime.ToString("MM");

            return month;
        }

        protected string GetWeekDiff(DateTime dateTime, DateTime basicDateTime)
        {
            TimeSpan timeSpan = dateTime - basicDateTime;
            int diffDays = timeSpan.Days;
            int diffWeek = (diffDays / 7) + 1;

            string str = diffWeek.ToString();
            return str;
        }
    }
}