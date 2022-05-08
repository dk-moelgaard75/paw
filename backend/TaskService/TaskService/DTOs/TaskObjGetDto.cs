using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.DTOs
{
    public class TaskObjGetDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int EstimatedHours { get; set; }
        public DateTime EndDate { get; set; }
        
        public Guid TaskGuid { get; set; }
        public Guid CustomerGuid { get; set; }

    }
}
