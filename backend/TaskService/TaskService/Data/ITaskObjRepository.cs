using TaskService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.Data
{
    public interface ITaskObjRepository
    {
		int Create(TaskObject newEntity);
		IEnumerable<TaskObject> GetAll();
		IEnumerable<TaskObject> GetByStartDate(DateTime startDate);
		TaskObject GetById(int id);
		TaskObject GetByCustomerGuid(Guid customerGuid);
		TaskObject GetByTaskGuid(Guid taskGuid);
		void Update(TaskObject modifiedEntity);
		void Delete(TaskObject entityToDelete);


		int CreateEmployee(TaskXEmployee newEntity);
		IEnumerable<TaskXEmployee> GetAllEmployees(Guid taskGuid);
		TaskXEmployee GetEmployeeByGuid(Guid customerGuid);
		void UpdateEmployee(TaskXEmployee modifiedEntity);
		void DeleteEmployee(TaskXEmployee entityToDelete);

	}
}
