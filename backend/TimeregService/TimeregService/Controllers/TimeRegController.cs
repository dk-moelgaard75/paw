using EmployeeService.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeregService.Data;
using TimeregService.Models;

namespace TimeregService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeRegController : ControllerBase
    {
        private readonly ITimeRegRepository _timeregRepository;
        public TimeRegController(ITimeRegRepository repository)
        {
            _timeregRepository = repository;
        }
        [HttpPost]
        public ActionResult Post([FromBody] TimeRegistration timereg)
        {
            PawLogger.DoLog("TimeregService kaldt");
            if (timereg == null)
            {
                Console.WriteLine($"TimeRegistrationService - timereg is null:");
            }
            PawLogger.DoLog("Opretter tidsregistrering");
            _timeregRepository.Create(timereg);
            PawLogger.DoLog("Returnere OK");
            return Ok();
        }
    }
}
