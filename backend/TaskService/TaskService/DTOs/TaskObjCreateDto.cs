using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskService.DTOs
{
    public class TaksObjCreateDto
    {
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int EstimatedDays { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public Guid CustomerGuid { get; set; }
    }
}
