using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Models
{
    public class CalendarModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public Guid CalenderGuid { get; set; }

        public int EmployeeDone { get; set; }

        public Guid TaskDone { get; set; }
    }
}
