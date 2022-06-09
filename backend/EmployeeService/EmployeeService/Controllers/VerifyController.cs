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
    public class VerifyController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public VerifyController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [AllowAnonymous]
        [HttpGet("{email}", Name = "GetByEmail")]
        public ActionResult GetByEmail(string email)
        {
            PawLogger.DoLog($"EmployeeService - Http GET recieved with email: {email}");
            Employee emp = _employeeRepository.GetByEmail(email);
            if (emp != null)
            {
                return Ok();
            } 
            return NotFound();
            //return JsonSerializer.Serialize(emp);
        }

    }
}
