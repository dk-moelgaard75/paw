using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalendarService.AsyncDataService;
using CalendarService.EventProcessing;
using CalendarService.DTOs;
using System.Globalization;
using CalendarService.Data;
using CalendarService.Models;

namespace CalendarService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMessageBusClient _messageBusClient;
        private readonly IEventProcessor _eventProcessor;
        private readonly ICalendarRepository _calendarRepository;
        
        public CalendarController(IMessageBusClient msgBusClient, 
                                IEventProcessor processor,
                                ICalendarRepository calendarRepository)
        {
            _messageBusClient = msgBusClient;
            _eventProcessor = processor;
            _calendarRepository = calendarRepository;
        }
        [HttpGet("{guid}/{startdate}", Name="GetCalendarWithId")] //URL: localhost/calendar/0340190f-aa45-4589-87ab-6a3838693f88/2022-05-31
        public string GetCalendarWithId(string guid, string startdate)
        {
            DateTime dt = DateTime.ParseExact(startdate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string html = _eventProcessor.GetCalendarHtml(guid, dt);
            if (html == null)
            {
                return "";
            }
            return html;
        }
        [HttpPost("{startdate}")]
        public string Post(string startdate)
        {
            Guid guid = Guid.NewGuid();
            CalendarModel model = new CalendarModel();
            model.CalenderGuid = guid;
            //Writes the calendar object to DB - later we can write EmployeeDone and TaskDone values to the table to indicate 
            //that data has arrived from RabbitM
            _calendarRepository.CreateCalendar(model);
            DateTime dt = DateTime.ParseExact(startdate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            _messageBusClient.RequestEmployees(guid.ToString());
            _messageBusClient.RequestTasks(dt, guid.ToString());
            return guid.ToString();
        }
    }
}
