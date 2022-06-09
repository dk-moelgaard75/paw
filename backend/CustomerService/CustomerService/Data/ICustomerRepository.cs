using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Data
{
    public interface ICustomerRepository
    {
		int Create(Customer newEntity);
		IEnumerable<Customer> GetAll();
		Customer GetById(int id);
		Customer GetByGuid(Guid id);
		Customer GetByEmail(string email);
		void Update(Customer modifiedEntity);
		void Delete(Customer entityToDelete);
	}
}
