﻿using AutoMapper;
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
            IEnumerable<Employee> emps = _employeeRepository.GetAll();
            //return JsonSerializer.Serialize(emps);
            return Ok(_mapper.Map<IEnumerable<EmployeeGetDto>>(emps));
        }
        [HttpGet("{id}",Name="GetById")]
        public ActionResult<EmployeeGetDto> GetById(int id)
        {
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
        public ActionResult<EmployeeGetDto> Post(EmployeeCreateDto employeeCreateDto)
        {
            EmployeeGetDto epmloyeeDto = null;
            try
            {
                //Map incomming DTO to a internal model object
                var employeeModel = _mapper.Map<Employee>(employeeCreateDto);
                
                //Use the internal model object to create a DbContext object
                var createdEmployee = _employeeRepository.Create(employeeModel);

                //Map the DbContext object to a DTO object (removing potential personal or otherwise sensitive data)
                epmloyeeDto = _mapper.Map<EmployeeGetDto>(createdEmployee);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            //Return information afbout URI (name=GetByID - HttpGet), Id for URI, and the created DTO
            return CreatedAtRoute(nameof(GetById), new { Id = epmloyeeDto.Id }, epmloyeeDto);
        }
        /* HttpPut checks if an instance exists - if so, the instance is updated - otherwise a new instance is created*/
        [HttpPut]
        public void Put(Employee employee)
        {
            Employee emp = _employeeRepository.GetById(employee.Id);
            if (emp != null)
            {
                //TODO - is this the right way? - could AutoMapper be used instead?
                emp.FirstName = employee.FirstName;
                emp.LastName = employee.LastName;
                emp.Email = employee.Email;
                _employeeRepository.Update(emp);
            }
            else
            {
                _employeeRepository.Create(employee);
            }
        }
        [HttpDelete]
        public void Delete(Employee employee)
        {
            _employeeRepository.Delete(employee);
        }

    }
}
