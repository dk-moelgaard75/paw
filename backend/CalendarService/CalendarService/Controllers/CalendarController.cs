﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMessageBusClient _messageBusClient;
        public CalendarController(IMessageBusClient msgBusClient)
        {

        }
        [HttpGet]
        public string GetCalendar()
        {
            return "";
        }
    }
}
