using Serilog;
using System;

namespace Infrastructure.Common
{
    public class Converters
    {
        public static DateTime ConvertString14ToDateTime(string? strdate)
        {
            DateTime retDate;
            int Year;
            int Month;
            int Day;
            int Hour;
            int Minute;
            int Second;
            try
            {
                if (string.IsNullOrEmpty(strdate))
                {
                    retDate = DateTime.MinValue;
                    return retDate;
                }

                Year = int.Parse(strdate.Substring(0, 4));
                Month = int.Parse(strdate.Substring(4, 2));
                Day = int.Parse(strdate.Substring(6, 2));
                Hour = int.Parse(strdate.Substring(8, 2));
                Minute = int.Parse(strdate.Substring(10, 2));
                Second = int.Parse(strdate.Substring(12, 2));
                retDate = new DateTime(Year, Month, Day, Hour, Minute, Second);
                return retDate;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                retDate = DateTime.MinValue;
                return retDate;

            }
        }
        public static string ConvertDateToString14(DateTime curDateTime)
        {
            string retval = curDateTime.ToString("yyyyMMddHHmmss");

            return retval;
        }
    }
}
