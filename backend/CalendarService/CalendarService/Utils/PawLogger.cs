using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Utils
{
    public static class PawLogger
    {
        public static void DoLog(string message)
        {
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                System.Diagnostics.Debug.Print(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
