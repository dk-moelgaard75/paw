using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.Utils
{
    public static class DateUtil
    {
        public static DateTime CalcEndDate(DateTime start, int daysToAdd)
        {
            return AddBusinessDays(start, daysToAdd);
        }

        //AddBusinessDay is found here:
        //https://stackoverflow.com/questions/279296/adding-days-to-a-date-but-excluding-weekends
        //GitHub:
        //https://github.com/FluentDateTime/FluentDateTime/blob/master/src/FluentDateTime/DateTime/DateTimeExtensions.cs
        private static DateTime AddBusinessDays(this DateTime current, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                } while (current.DayOfWeek == DayOfWeek.Saturday ||
                         current.DayOfWeek == DayOfWeek.Sunday);
            }
            return current;
        }
    }
}
