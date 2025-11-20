using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FOOD.Utility
{
    public class UtilityHelper
    {
       

        public static DateTime ToLocalDateandTime(DateTime? date)
        {
            var TimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc((DateTime)date, TimeZone);
        }
    }
}
