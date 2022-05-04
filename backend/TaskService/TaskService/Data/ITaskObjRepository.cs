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
		TaskObject GetById(int id);
		TaskObject GetByGuid(Guid customerGuid);
		void Update(TaskObject modifiedEntity);
		void Delete(TaskObject entityToDelete);
	}
}
