using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.DTOs
{
    public class EmployeePublishedDto
    {
        public Guid EmployeeGuid { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
