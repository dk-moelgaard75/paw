using CalendarService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Data
{
    public class CalendarRepository : ICalendarRepository
    {
        public CalendarRepository()
        {

        }
        public int CreateCalendar(CalendarModel calendarModel)
        {
            throw new NotImplementedException();
        }

        public int CreateCalendarEmployee(CalendarEmployeeModel calendarEmployeeModel)
        {
            throw new NotImplementedException();
        }

        public int CreateCalendarTaskObj(CalendarTaskObjModel calendarTaskObjModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteCalendar(CalendarModel calendarEmployeeModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteCalendarEmployee(CalendarEmployeeModel calendarEmployeeModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteTaskObjCalendar(CalendarTaskObjModel calendarEmployeeModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CalendarModel> GetAllCalendar()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CalendarEmployeeModel> GetAllCalendarEmployee()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CalendarTaskObjModel> GetAllTaskObjCalendar()
        {
            throw new NotImplementedException();
        }

        public CalendarModel GetCalendarByCalendarGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public CalendarEmployeeModel GetCalendarEmployeeByCalendarGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public CalendarModel GetCalendarTaskObjByCalendarGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateCalendar(CalendarModel calendarEmployeeModel)
        {
            throw new NotImplementedException();
        }

        public void UpdateCalendarEmployee(CalendarEmployeeModel calendarEmployeeModel)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskObjCalendar(CalendarTaskObjModel calendarEmployeeModel)
        {
            throw new NotImplementedException();
        }
    }
}
