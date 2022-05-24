using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeregService.Models;

namespace TimeregService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeRegController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] TimeRegistration timereg)
        {
            if (timereg == null)
            {
                Console.WriteLine($"TimeRegistrationService - timereg is null:");
            }
            return Ok();
        }
    }
}
