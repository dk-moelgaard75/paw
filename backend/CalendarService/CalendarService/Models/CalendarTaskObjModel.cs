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
        public Guid CalenderGuid { get; set; }

        public Guid TaskGuid { get; set; }
        public DateTime StartDatetime { get; set; }

        public int StartHour { get; set; }
        public int EstimatedHours { get; set; }
    }
}
