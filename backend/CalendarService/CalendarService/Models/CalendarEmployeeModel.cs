using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Models
{
    public class CalendarEmployeeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid CalendarGuid { get; set; }
        public Guid EmployeeGuid { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid CalenderGuid { get; set; }
        /*
        public Guid EmployeeGuid { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid CalendarGuid { get; set; }
         */
    }
}
