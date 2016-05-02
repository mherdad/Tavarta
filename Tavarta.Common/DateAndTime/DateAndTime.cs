using System;
using Persia;

namespace Tavarta.Common.DateAndTime
{
    public class DateAndTime
    {
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public static string ConvertToPersian(DateTime dateTime, string mod = "")
        {
            SolarDate solar = Calendar.ConvertToPersian(dateTime);
            return string.IsNullOrEmpty(mod) ? solar.ToString() : solar.ToString(mod);
        }
    }
}