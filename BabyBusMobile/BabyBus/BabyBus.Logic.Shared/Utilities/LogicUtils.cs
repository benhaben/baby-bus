using System;
using System.Text.RegularExpressions;

namespace BabyBus.Logic.Shared
{
    public static class LogicUtils
    {
        private static DateTime _startTime = new DateTime(1970, 1, 1).AddHours(8);

        public static DateTime ConvertIntDateTime(int d)
        {
            DateTime time = _startTime.AddSeconds(d);
            return time;
        }

        /// <summary>
        /// DateTime转Int（TimeStamp）
        /// </summary>
        /// <returns></returns>
        public static int ConvertDateTimeInt(DateTime d)
        {
            var stamp = Convert.ToInt32((d - _startTime).TotalSeconds);
            return stamp;
        }

        /// <summary>
        /// DateTime转String
        /// </summary>
        public static string DateTimeString(DateTime datetime)
        {
            var timeSpan = DateTime.Now.Date - datetime.Date;
            if (timeSpan.Days == 0)
            {
                return string.Format("今天") + datetime.ToString("t");
            }
            if (timeSpan.Days == 1)
            {
                return string.Format("昨天 ") + datetime.ToString("t");
            }
            return datetime.ToString("yyyy年M月d日");
        }

        /// <summary>
        /// DateTime转Int（TimeStamp），默认当前时间
        /// </summary>
        /// <returns></returns>
        public static int ConvertDateTimeInt()
        {
            return ConvertDateTimeInt(DateTime.Now);
        }
    }
}