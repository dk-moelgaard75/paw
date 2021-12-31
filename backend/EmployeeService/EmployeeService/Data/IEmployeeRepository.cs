using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.Data
{
	public interface IEmployeeRepository
	{
		int Create(Employee newEntity);
		IEnumerable<Employee> GetAll();
		Employee GetById(int id);
		void Update(Employee modifiedEntity);
		void Delete(Employee entityToDelete);
	}
}
