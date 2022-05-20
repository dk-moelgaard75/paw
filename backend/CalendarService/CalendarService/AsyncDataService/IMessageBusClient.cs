using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.AsyncDataService
{
    public interface IMessageBusClient
    {
        void RequestEmployees(string calendarGuid);
        void RequestEmployee(string searchField, string searchValue);
        void RequestTasks();
        void RequestTasks(DateTime startDate, string calendarGuid);

    }
}
