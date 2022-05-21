using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Models
{
    public class CalendarTaskObjModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Guid TaskGuid { get; set; }
        public Guid CalendarGuid { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public int StartTime { get; set; }
        public int EstimatedHours { get; set; }
        public Guid Employee { get; set; }

        /*
        //DTO - for comparison 
        public Guid TaskGuid { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public int StartTime { get; set; }
        public int EstimatedHours { get; set; }
        public Guid CalendarGuid { get; set; }
        public Guid Employee { get; set; }
        */
    }
}
