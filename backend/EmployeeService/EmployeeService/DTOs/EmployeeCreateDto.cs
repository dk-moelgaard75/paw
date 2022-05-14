using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.DTOs
{
    public class EmployeeCreateDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Phone { get; set; }
        [Required]

        public string EmployeeType { get; set; }
        public string Password { get; set; }

    }
}
