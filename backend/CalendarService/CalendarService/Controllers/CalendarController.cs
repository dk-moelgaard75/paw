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

namespace CalendarService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMessageBusClient _messageBusClient;
        private readonly IEventProcessor _eventProcessor;
        public CalendarController(IMessageBusClient msgBusClient, IEventProcessor processor)
        {
            _messageBusClient = msgBusClient;
            _eventProcessor = processor;
        }
        [HttpGet("{string:guid}", Name="GetCalendarWithId")]
        public IActionResult GetCalendarWithId(string guid)
        {
            string html = _eventProcessor.GetCalendarHtml(guid);
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
            DateTime dt = DateTime.ParseExact(startDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //_messageBusClient.RequestEmployees(guid.ToString());
            _messageBusClient.RequestTasks(dt, guid.ToString());
            return Ok(guid.ToString());
        }
    }
}
