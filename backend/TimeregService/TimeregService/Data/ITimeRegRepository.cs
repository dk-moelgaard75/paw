using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeregService.Models;

namespace TimeregService.Data
{
    public interface ITimeRegRepository
    {
		int Create(TimeRegistration newEntity);
		IEnumerable<TimeRegistration> GetAll();
		TimeRegistration GetById(int id);
		void Update(TimeRegistration modifiedEntity);
		void Delete(TimeRegistration entityToDelete);

	}
}
