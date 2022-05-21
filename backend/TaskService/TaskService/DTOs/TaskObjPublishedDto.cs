using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.DTOs
{
    public class TaskObjPublishedDto
    {
        public Guid TaskGuid { get; set; }
        public string TaskName { get; set; }

        public DateTime StartDate { get; set; }
        public int StartTime { get; set; }
        public int EstimatedHours { get; set; }
        
        public Guid CalendarGuid { get; set; }
        
        public Guid Employee { get; set; }
    }
}
