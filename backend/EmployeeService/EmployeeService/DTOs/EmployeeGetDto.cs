using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.DTOs
{
    public class EmployeeGetDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmployeeType { get; set; }
        public string Password { get; set; }
        public Guid UID { get; set; }
    }

}
