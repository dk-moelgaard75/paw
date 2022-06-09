using AutoMapper;
using EmployeeService.AsyncDataServices;
using EmployeeService.Data;
using EmployeeService.DTOs;
using EmployeeService.Models;
using EmployeeService.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public EmployeeController(IEmployeeRepository employeeRepository, 
                                    IMapper mapper,
                                    IMessageBusClient msgBusClient)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _messageBusClient = msgBusClient;
        }

        [HttpGet(Name="GetAll")]
        public ActionResult<IEnumerable<EmployeeGetDto>> GetAll()
        {
            Console.WriteLine("EmployeeService - Http GET recieved");
            IEnumerable<Employee> emps = _employeeRepository.GetAll();
            //return JsonSerializer.Serialize(emps);
            return Ok(_mapper.Map<IEnumerable<EmployeeGetDto>>(emps));
        }
        [HttpGet("{id}",Name="GetById")]
        public ActionResult<EmployeeGetDto> GetById(int id)
        {
            PawLogger.DoLog($"EmployeeService - Http GET recieved with ID: {id}");
            Employee emp = _employeeRepository.GetById(id);
            if (emp != null)
            {
                return Ok(_mapper.Map<EmployeeGetDto>(emp));
            }
            return NotFound();
            //return JsonSerializer.Serialize(emp);
        }


        /* HttpPost creates new instance*/
        [HttpPost]
        public ActionResult<EmployeeGetDto> Post([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            if (employeeCreateDto == null)
            {
                Console.WriteLine($"employeeCreateDto is null:");
            }
            //EmployeeCreateDto tmp = JsonSerializer.Deserialize<EmployeeCreateDto>(employeeCreateDto.ToString());
            PawLogger.DoLog($"Http POST recieved with data:");
            PawLogger.DoLog($"{employeeCreateDto.FirstName}");
            PawLogger.DoLog($"{employeeCreateDto.LastName}");
            PawLogger.DoLog($"{employeeCreateDto.Email}");
            PawLogger.DoLog($"{employeeCreateDto.Phone}");
            PawLogger.DoLog($"{employeeCreateDto.Password}");
            EmployeeGetDto epmloyeeDto = null;
            try
            {
                //Map incomming DTO to a internal model object
                Employee employeeModel = _mapper.Map<Employee>(employeeCreateDto);
                PawLogger.DoLog($"Employee model created:");
                PawLogger.DoLog($"{employeeModel.FirstName}");
                PawLogger.DoLog($"{employeeModel.LastName}");
                PawLogger.DoLog($"{employeeModel.Email}");
                PawLogger.DoLog($"{employeeModel.Phone}");
                PawLogger.DoLog($"{employeeModel.Password}");

                //setting UID
                employeeModel.UID = Guid.NewGuid();

                //Use the internal model object to create a DbContext object
                int createdEmployeeId = _employeeRepository.Create(employeeModel);

                //Map the DbContext object to a DTO object (removing potential personal or otherwise sensitive data)
                epmloyeeDto = _mapper.Map<EmployeeGetDto>(employeeModel);
            }
            catch (Exception ex)
            {
                PawLogger.DoLog($"Http POST exception: {ex.Message}" );
                return StatusCode(500);
            }
            try
            {
                EmployeePublishedDto emp = _mapper.Map<EmployeePublishedDto>(epmloyeeDto);
                _messageBusClient.PublishEmployee(emp);
            }
            catch(Exception ex)
            {
                Console.Write("Error in EmployeeService - HttpPost:" + ex.Message);
            }
            //For testing purpose
            if (employeeCreateDto.FirstName.Equals("John") && employeeCreateDto.FirstName.Equals("Doe"))
            {
                return StatusCode(500);
            }
            else
            {
                //Return information afbout URI (name=GetByID - HttpGet), Id for URI, and the created DTO
                return CreatedAtRoute(nameof(GetById), new { Id = epmloyeeDto.Id }, epmloyeeDto);
            }
        }
        /* HttpPut checks if an instance exists - if so, the instance is updated - otherwise a new instance is created*/
        [HttpPut]
        public IActionResult Put([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            PawLogger.DoLog($"Http PUT recieved with data:");
            PawLogger.DoLog($"{employeeCreateDto.Email}");

            Employee emp = _employeeRepository.GetByEmail(employeeCreateDto.Email);
            try
            {
                if (emp != null)
                {
                    PawLogger.DoLog($"Found employee based on email:" + employeeCreateDto.Email);
                    //Map properties of src (first argument) to dest (second argument)
                    _mapper.Map(employeeCreateDto, emp);
                    _employeeRepository.Update(emp);
                }
                else
                {
                    var employeeModel = _mapper.Map<Employee>(employeeCreateDto);
                    _employeeRepository.Create(employeeModel);
                }
            }
            catch (Exception ex)
            {
                PawLogger.DoLog("Error in EmployeeControler - Put:" + ex.Message);
                return StatusCode(500);
            }
            return Ok();
        }
        [HttpDelete("{emplId:int}")]
        public IActionResult Delete(int emplId)
        {
            PawLogger.DoLog("EmployeeController - Delete hit with ID:" + emplId);
            Employee empl = _employeeRepository.GetById(emplId);
            if (empl != null)
            {
                _employeeRepository.Delete(empl);
                return Ok();
            }
            return NotFound();
            
        } 

    }
}
