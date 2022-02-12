using System;

namespace Model.Helpers
{
    /// <summary> Converts </summary>
    public partial class Util
    {
        // Convert string to date
        public static DateTime ConvertStringToDate(string strDate)
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
                Year = int.Parse(strDate.Substring(0, 4));
                Month = int.Parse(strDate.Substring(4, 2));
                Day = int.Parse(strDate.Substring(6, 2));
                Hour = int.Parse(strDate.Substring(8, 2));
                Minute = int.Parse(strDate.Substring(10, 2));
                Second = int.Parse(strDate.Substring(12, 2));

                retDate = new DateTime(Year, Month, Day, Hour, Minute, Second);

                return retDate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Convert date to datetime
        public static DateTime ConvertDateToDateTime(string dateTime)
        {
            DateTime result = new DateTime();
            if (!string.IsNullOrEmpty(dateTime))
            {
                string[] arr = dateTime.Trim().Split(",");
                string[] date = arr[0].Split(".");
                string[] time = arr[1].Split(":");

                int ye = Convert.ToInt32(date[2]);
                int mo = Convert.ToInt32(date[1]);
                int da = Convert.ToInt32(date[0]);

                int ho = Convert.ToInt32(time[0]);
                int mi = Convert.ToInt32(time[1]);
                int se = Convert.ToInt32(time[2]);

                DateTime d = new DateTime(ye, mo, da, ho, mi, se);
                result = d;
            }
            return result;
        }
    }
}
