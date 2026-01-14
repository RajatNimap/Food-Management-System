using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FOOD.Utility
{
    public class UtilityHelper
    {
        public static DateTime? ToLocalDateandTime(DateTime? date)
        {
            // if input is null, just return null
            if (!date.HasValue)
                return null;

            // ensure it's treated as UTC
            var utc = DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utc, timeZone);
        }


    }
}
