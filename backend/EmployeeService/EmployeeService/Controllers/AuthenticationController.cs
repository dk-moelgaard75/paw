using EmployeeService.Data;
using EmployeeService.Models;
using EmployeeService.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public AuthenticationController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [AllowAnonymous]
        [HttpPost(Name = "Authenticate")]
        public ActionResult Authenticate([FromBody] EmployeeAuth auth)
        {
            PawLogger.DoLog("EmployeeService - Authenticate");
            PawLogger.DoLog(auth.email);
            PawLogger.DoLog(auth.password);
            Tokens token = _employeeRepository.Authenticate(auth.email, auth.password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
