using CalendarService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Data
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly CalendarDbContext _context;
        public CalendarRepository(CalendarDbContext context)
        {
            _context = context;
        }
        #region Calendar
        public int CreateCalendar(CalendarModel newEntity)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            _context.Calendar.Add(newEntity);
            _context.SaveChanges();
            return newEntity.Id;
        }
        public IEnumerable<CalendarModel> GetAllCalendar()
        {
            return _context.Calendar.ToList();
        }

        public CalendarModel GetCalendarByCalendarGuid(Guid id)
        {
            return _context.Calendar.FirstOrDefault(p => p.CalenderGuid == id);
        }

        public void UpdateCalendar(CalendarModel modifiedEntity)
        {
            _context.Calendar.Update(modifiedEntity);
            _context.SaveChanges();
        }

        public void DeleteCalendar(CalendarModel entityToDelete)
        {
            _context.Calendar.Remove(entityToDelete);
            _context.SaveChanges();
        }
        #endregion

        #region CalendarEmployee
        public int CreateCalendarEmployee(CalendarEmployeeModel newEntity)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            _context.CalendarEmployee.Add(newEntity);
            _context.SaveChanges();
            return newEntity.Id;

        }
        public IEnumerable<CalendarEmployeeModel> GetAllCalendarEmployee()
        {
            return _context.CalendarEmployee.ToList();
        }
        
        public CalendarEmployeeModel GetCalendarEmployeeByCalendarGuid(Guid id)
        {
            return _context.CalendarEmployee.FirstOrDefault(p => p.CalenderGuid == id);
        }

        public void UpdateCalendarEmployee(CalendarEmployeeModel modifiedEntity)
        {
            _context.CalendarEmployee.Remove(modifiedEntity);
            _context.SaveChanges();
        }

        public void DeleteCalendarEmployee(CalendarEmployeeModel entityToDelete)
        {
            _context.CalendarEmployee.Remove(entityToDelete);
            _context.SaveChanges();
        }
        #endregion



        #region CalendarTaskObj
        public int CreateCalendarTaskObj(CalendarTaskObjModel newEntity)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            _context.CalendarTaskObj.Add(newEntity);
            _context.SaveChanges();
            return newEntity.Id;

        }
        public IEnumerable<CalendarTaskObjModel> GetAllTaskObjCalendar()
        {
            return _context.CalendarTaskObj.ToList();
        }
        public CalendarTaskObjModel GetCalendarTaskObjByCalendarGuid(Guid id)
        {
            return _context.CalendarTaskObj.First(p => p.CalenderGuid == id);
        }

        public void UpdateTaskObjCalendar(CalendarTaskObjModel modifiedEntity)
        {
            _context.CalendarTaskObj.Update(modifiedEntity);
            _context.SaveChanges();
        }

        public void DeleteTaskObjCalendar(CalendarTaskObjModel entityToDelete)
        {
            _context.CalendarTaskObj.Remove(entityToDelete);
            _context.SaveChanges();
        }

        #endregion
    }
}
