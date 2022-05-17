using CalendarService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Data
{
    public interface ICalendarRepository
    {
		//CalendarEmployee entities
		int CreateCalendarEmployee(CalendarEmployeeModel calendarEmployeeModel);
		IEnumerable<CalendarEmployeeModel> GetAllCalendarEmployee();
		CalendarEmployeeModel GetCalendarEmployeeByCalendarGuid(Guid id);
		void UpdateCalendarEmployee(CalendarEmployeeModel calendarEmployeeModel);
		void DeleteCalendarEmployee(CalendarEmployeeModel calendarEmployeeModel);

		//Calendar entities
		int CreateCalendar(CalendarModel calendarModel);
		IEnumerable<CalendarModel> GetAllCalendar();
		CalendarModel GetCalendarByCalendarGuid(Guid id);
		void UpdateCalendar(CalendarModel calendarEmployeeModel);
		void DeleteCalendar(CalendarModel calendarEmployeeModel);

		//CalendarTaskObj entities
		int CreateCalendarTaskObj(CalendarTaskObjModel calendarTaskObjModel);
		IEnumerable<CalendarTaskObjModel> GetAllTaskObjCalendar();
		CalendarModel GetCalendarTaskObjByCalendarGuid(Guid id);
		void UpdateTaskObjCalendar(CalendarTaskObjModel calendarEmployeeModel);
		void DeleteTaskObjCalendar(CalendarTaskObjModel calendarEmployeeModel);
	}


}
