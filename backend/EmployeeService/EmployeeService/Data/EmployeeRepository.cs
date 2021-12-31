using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        public EmployeeRepository(EmployeeDbContext context) // Constructor DI Injection
        {
            _context = context;
        }
        public int Create(Employee newEntity)
        {
            //TODO - better exception handling
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            _context.Employees.Add(newEntity);
            _context.SaveChanges();

            return newEntity.Id;
        }

        public void Delete(Employee entityToDelete)
        {
            _context.Employees.Remove(entityToDelete);
            _context.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Employee modifiedEntity)
        {
            _context.Employees.Update(modifiedEntity);
            _context.SaveChanges();
        }
    }
}
