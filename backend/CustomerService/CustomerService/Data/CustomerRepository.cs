using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;
        public CustomerRepository(CustomerDbContext context) // Constructor DI Injection
        {
            _context = context;
        }
        public int Create(Customer newEntity)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            _context.Customers.Add(newEntity);
            _context.SaveChanges();

            return newEntity.Id;
        }

        public void Delete(Customer entityToDelete)
        {
            _context.Customers.Remove(entityToDelete);
            _context.SaveChanges();
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public Customer GetByEmail(string mail)
        {
            return _context.Customers.FirstOrDefault(p => p.Email.Equals(mail));
        }

        public Customer GetById(int id)
        {
            return _context.Customers.FirstOrDefault(p => p.Id == id);
        }
        public Customer GetByGuid(Guid id)
        {
            return _context.Customers.FirstOrDefault(p => p.UID == id);
        }

        public void Update(Customer modifiedEntity)
        {
            _context.Customers.Update(modifiedEntity);
            _context.SaveChanges();
        }
    }
}
