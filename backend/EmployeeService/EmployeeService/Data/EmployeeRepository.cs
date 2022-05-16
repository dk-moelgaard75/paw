using EmployeeService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly IConfiguration _configuration;
        public EmployeeRepository(EmployeeDbContext context, IConfiguration configuration) // Constructor DI Injection
        {
            _context = context;
            _configuration = configuration;
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
        public Employee GetByEmail(string mail)
        {
            return _context.Employees.FirstOrDefault(p => p.Email.Equals(mail));
        }

        public void Update(Employee modifiedEntity)
        {
            _context.Employees.Update(modifiedEntity);
            _context.SaveChanges();
        }

        public Tokens Authenticate(string email, string password)
        {
            Employee emp = _context.Employees.FirstOrDefault(p => p.Email.ToLower().Equals(email.ToLower()) && p.Password.Equals(password));
            if (emp == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, emp.FirstName),
                    new Claim(ClaimTypes.GivenName, emp.FirstName),
                    new Claim(ClaimTypes.Role, emp.EmployeeType),
                    new Claim(ClaimTypes.Email, emp.Email),
                    new Claim(ClaimTypes.Surname, emp.LastName)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { Token = tokenHandler.WriteToken(token) };

        }
    }
}
