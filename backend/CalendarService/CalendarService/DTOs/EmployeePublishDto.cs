using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.DTOs
{
    public class EmployeePublishDto
    {
        public Guid EmployeeGuid { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid CalendarGuid { get; set; }
    }
}
