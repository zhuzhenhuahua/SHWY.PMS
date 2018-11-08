using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHWY.Utility
{
   public static class CommonHelper
    {
        public static string GetRandomString(string startWith)
        {
            Random r = new Random();
            return startWith + DateTime.Now.ToString("yyyyMMddHHmmss")+ r.Next(1000, 9999).ToString();
        }
        #region 本机信息
        public static string GetHostName()
        {
            return Environment.MachineName;
        }

        public static string GetLoggerName(Type t)
        {
            return t.ToString();
        }

        public static string GetThreadId()
        {
            return System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
        }

        public static string GetOSName()
        {
            return Environment.OSVersion.ToString();
        }

        public static string GetDomain()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }
        #endregion
        #region 获取 本周、本月、本季度、本年 的开始时间或结束时间
        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? GetTimeStartByType(ETimeType TimeType, DateTime now)
        {
            switch (TimeType)
            {
                case ETimeType.Week:
                    return now.AddDays(-(int)now.DayOfWeek + 1);
                case ETimeType.Month:
                    return now.AddDays(-now.Day + 1);
                case ETimeType.Season:
                    var time = now.AddMonths(0 - ((now.Month - 1) % 3));
                    return time.AddDays(-time.Day + 1);
                case ETimeType.Year:
                    return now.AddDays(-now.DayOfYear + 1);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? GetTimeEndByType(ETimeType TimeType, DateTime now)
        {
            switch (TimeType)
            {
                case ETimeType.Week:
                    return now.AddDays(7 - (int)now.DayOfWeek);
                case ETimeType.Month:
                    return now.AddMonths(1).AddDays(-now.AddMonths(1).Day + 1).AddDays(-1);
                case ETimeType.Season:
                    var time = now.AddMonths((3 - ((now.Month - 1) % 3) - 1));
                    return time.AddMonths(1).AddDays(-time.AddMonths(1).Day + 1).AddDays(-1);
                case ETimeType.Year:
                    var time2 = now.AddYears(1);
                    return time2.AddDays(-time2.DayOfYear);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取指定日期，在为一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrn>返回第几周</reutrn>
        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }
        /// <summary>
        /// 获取月份
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetMonthOfYear(DateTime dt)
        {
            return dt.Month;
        }
        #endregion
    }
    public enum ETimeType {
        Week,
        Month,
        Season,
        Year
    }
}
