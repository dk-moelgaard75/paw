using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.DTOs
{
    public class CalenderRequestDto
    {
        public DateTime StartDate { get; set; }
        public string CalendarGuid { get; set; }

    }
}
