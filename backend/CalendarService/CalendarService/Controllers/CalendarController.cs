using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalendarService.AsyncDataService;

namespace CalendarService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMessageBusClient _messageBusClient;
        public CalendarController(IMessageBusClient msgBusClient)
        {
            _messageBusClient = msgBusClient;
        }
        [HttpGet]
        public string GetCalendar()
        {
            _messageBusClient.RequestEmployees();
            return "";
        }
    }
}
