using AutoMapper;
using EmployeeService.Data;
using EmployeeService.DTOs;
using EmployeeService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }
        [HttpGet(Name="GetAll")]
        public ActionResult<IEnumerable<EmployeeGetDto>> GetAll()
        {
            Console.WriteLine("Http GET recieved");
            IEnumerable<Employee> emps = _employeeRepository.GetAll();
            //return JsonSerializer.Serialize(emps);
            return Ok(_mapper.Map<IEnumerable<EmployeeGetDto>>(emps));
        }
        [HttpGet("{id}",Name="GetById")]
        public ActionResult<EmployeeGetDto> GetById(int id)
        {
            Console.WriteLine($"Http GET recieved with ID: {id}");
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
            Console.WriteLine($"Http POST recieved with data:");
            Console.WriteLine($"{employeeCreateDto.FirstName}");
            Console.WriteLine($"{employeeCreateDto.LastName}");
            Console.WriteLine($"{employeeCreateDto.Email}");
            EmployeeGetDto epmloyeeDto = null;
            try
            {
                //Map incomming DTO to a internal model object
                Employee employeeModel = _mapper.Map<Employee>(employeeCreateDto);
                Console.WriteLine($"Employee model created:");
                Console.WriteLine($"{employeeModel.FirstName}");
                Console.WriteLine($"{employeeModel.LastName}");
                Console.WriteLine($"{employeeModel.Email}");

                //setting UID
                employeeModel.UID = Guid.NewGuid();

                //Use the internal model object to create a DbContext object
                int createdEmployeeId = _employeeRepository.Create(employeeModel);

                //Map the DbContext object to a DTO object (removing potential personal or otherwise sensitive data)
                epmloyeeDto = _mapper.Map<EmployeeGetDto>(employeeModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Http POST exception: {ex.Message}" );
                return StatusCode(500);
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
        public void Put(EmployeeCreateDto employeeCreateDto)
        {
            Employee emp = _employeeRepository.GetByEmail(employeeCreateDto.Email);
            if (emp != null)
            {
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
        [HttpDelete]
        public void Delete(Employee employee)
        {
            _employeeRepository.Delete(employee);
        }

    }
}
