using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskService.Models;

namespace TaskService.Data
{
    public class TaskObjRepository : ITaskObjRepository
    {
        private readonly TaskObjDbContext _context;
        private readonly IConfiguration _configuration;
        public TaskObjRepository(TaskObjDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public int Create(TaskObject newEntity)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            _context.TaskObjs.Add(newEntity);
            _context.SaveChanges();

            return newEntity.Id;
        }


        public void Delete(TaskObject entityToDelete)
        {
            _context.TaskObjs.Remove(entityToDelete);
            _context.SaveChanges();
        }


        public IEnumerable<TaskObject> GetAll()
        {
            return _context.TaskObjs.ToList();
        }
        public IEnumerable<TaskObject> GetByStartDate(DateTime startDate)
        {
            string confDays = _configuration["CalenderSearchNrOfDays"];
            double addDays = 0;
            if (!Double.TryParse(confDays, out addDays))
            {
                addDays = 7;
            }
            DateTime endDate = startDate.AddDays(addDays);
            return _context.TaskObjs.Where(p => p.StartDate >= startDate.Date && p.StartDate <= endDate.Date);
        }
        public IEnumerable<TaskObject> GetByEmployyeId(Guid emplGuid) 
        {
            //return _context.TaskObjs.Where(p => p.StartDate >= startDate.Date && p.StartDate <= endDate.Date);
            IEnumerable<TaskObject> dbResult = (from t in _context.TaskObjs
                                                join e in _context.TaskXEmployee on t.TaskGuid equals e.TaskGuid
                                                where e.EmployeeGuid == emplGuid
                                                orderby t.StartDate, t.StartHour
                                                select new TaskObject
                                                {
                                                    Id = t.Id,
                                                    TaskGuid = t.TaskGuid,
                                                    TaskName = t.TaskName,
                                                    Description = t.Description,
                                                    StartDate = t.StartDate,
                                                    StartHour = t.StartHour,
                                                    EstimatedHours = t.EstimatedHours,
                                                    CustomerGuid = t.CustomerGuid,
                                                    Employee = e.EmployeeGuid
                                                }).AsEnumerable<TaskObject>();
            return dbResult;

        }

        public IEnumerable<TaskXEmployee> GetAllEmployees(Guid taskGuid)
        {
            return _context.TaskXEmployee.Where(p => p.TaskGuid.Equals(taskGuid));
            
        }

        public TaskObject GetByCustomerGuid(Guid customerGuid)
        {
            return _context.TaskObjs.FirstOrDefault(p => p.CustomerGuid.Equals(customerGuid));
        }
        public TaskObject GetByTaskGuid(Guid customerGuid)
        {
            return _context.TaskObjs.FirstOrDefault(p => p.TaskGuid.Equals(customerGuid));
        }

        public TaskObject GetById(int id)
        {
            return _context.TaskObjs.FirstOrDefault(p => p.Id == id);
        }


        public void Update(TaskObject modifiedEntity)
        {
            _context.TaskObjs.Update(modifiedEntity);
            _context.SaveChanges();
        }


        public int CreateEmployee(TaskXEmployee newEntity)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            _context.TaskXEmployee.Add(newEntity);
            _context.SaveChanges();

            return newEntity.Id;
        }

        public TaskXEmployee GetEmployeeByGuid(Guid empGuid)
        {
            return _context.TaskXEmployee.FirstOrDefault(p => p.EmployeeGuid.Equals(empGuid));
        }

        public void UpdateEmployee(TaskXEmployee modifiedEntity)
        {
            _context.TaskXEmployee.Update(modifiedEntity);
            _context.SaveChanges();

        }
        public void DeleteEmployee(TaskXEmployee entityToDelete)
        {
            _context.TaskXEmployee.Remove(entityToDelete);
            _context.SaveChanges();
        }

    }
}
