using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.EventProcessing
{
    public interface IEventProcessor
    {
        public string GetCalendarHtml(string guid, DateTime dateTime);
    }
}
