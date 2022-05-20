using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskService.DTOs
{
    public class TaskObjCreateDto
    {
        [Required]
        public string TaskName { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int StartHour { get; set; }

        [Required]
        public int EstimatedHours { get; set; }
                
        [Required]
        public Guid CustomerGuid { get; set; }

        public Guid Employee { get; set; }
    }
}
