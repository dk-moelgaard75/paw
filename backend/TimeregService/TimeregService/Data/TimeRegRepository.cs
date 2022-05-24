using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeregService.Models;

namespace TimeregService.Data
{
    public class TimeRegRepository : ITimeRegRepository
    {
        private TimeRegDbContext _context;
        public TimeRegRepository(TimeRegDbContext context)
        {
            _context = context;
        }
        public int Create(TimeRegistration newEntity)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            _context.TimeReg.Add(newEntity);
            _context.SaveChanges();

            return newEntity.Id;

        }

        public void Delete(TimeRegistration entityToDelete)
        {
            _context.TimeReg.Remove(entityToDelete);
            _context.SaveChanges();

        }

        public IEnumerable<TimeRegistration> GetAll()
        {
            return _context.TimeReg.ToList();
        }

        public TimeRegistration GetById(int id)
        {
            return _context.TimeReg.FirstOrDefault(p => p.Id == id);
        }

        public void Update(TimeRegistration modifiedEntity)
        {
            _context.TimeReg.Update(modifiedEntity);
            _context.SaveChanges();

        }
    }
}
