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
        [HttpGet("{string:guid}", Name="GetCalendarWithId")]
        public IActionResult GetCalendarWithId([FromBody] object startdate)
        {
            DateTime dt = DateTime.ParseExact(startdate.ToString(), "yyyy-MM-dd",CultureInfo.InvariantCulture);
            string html = _eventProcessor.GetCalendarHtml(guid, dt);
            if (html == null)
            {
                return NotFound();
            }

            return Ok(html);
        }
        [HttpPost]
        public IActionResult Post([FromBody] object startDate)
        {
            Guid guid = Guid.NewGuid();
            CalendarModel model = new CalendarModel();
            model.CalenderGuid = guid;
            //Writes the calendar object to DB - later we can write EmployeeDone and TaskDone values to the table to indicate 
            //that data has arrived from RabbitM
            _calendarRepository.CreateCalendar(model);
            DateTime dt = DateTime.ParseExact(startDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            _messageBusClient.RequestEmployees(guid.ToString());
            _messageBusClient.RequestTasks(dt, guid.ToString());
            return Ok(guid.ToString());
        }
    }
}
