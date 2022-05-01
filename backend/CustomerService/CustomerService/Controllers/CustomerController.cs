using CustomerService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.DTOs;
using CustomerService.Models;

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        [HttpGet(Name = "GetAll")]
        public ActionResult<IEnumerable<CustomerGetDto>> GetAll()
        {
            Console.WriteLine("Http GET recieved");
            IEnumerable<Customer> emps = _customerRepository.GetAll();
            //return JsonSerializer.Serialize(emps);
            return Ok(_mapper.Map<IEnumerable<CustomerGetDto>>(emps));
        }
        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<CustomerGetDto> GetById(int id)
        {
            Console.WriteLine($"Http GET recieved with ID: {id}");
            Customer emp = _customerRepository.GetById(id);
            if (emp != null)
            {
                return Ok(_mapper.Map<CustomerGetDto>(emp));
            }
            return NotFound();
        }
        /* HttpPost creates new instance*/
        [HttpPost]
        public ActionResult<CustomerGetDto> Post([FromBody] CustomerCreateDto customerCreateDto)
        {
            if (customerCreateDto == null)
            {
                Console.WriteLine($"CustomerCreateDto is null:");
            }
            //EmployeeCreateDto tmp = JsonSerializer.Deserialize<EmployeeCreateDto>(employeeCreateDto.ToString());
            Console.WriteLine($"Http POST recieved with data:");
            Console.WriteLine($"{customerCreateDto.FirstName}");
            Console.WriteLine($"{customerCreateDto.LastName}");
            Console.WriteLine($"{customerCreateDto.Email}");
            Console.WriteLine($"{customerCreateDto.Phone}");
            Console.WriteLine($"{customerCreateDto.Address}");
            Console.WriteLine($"{customerCreateDto.Zip}");
            Console.WriteLine($"{customerCreateDto.Country}");
            CustomerGetDto customerDto = null;
            try
            {
                //Map incomming DTO to a internal model object
                Customer customerModel = _mapper.Map<Customer>(customerCreateDto);
                Console.WriteLine($"Employee model created:");
                Console.WriteLine($"{customerModel.FirstName}");
                Console.WriteLine($"{customerModel.LastName}");
                Console.WriteLine($"{customerModel.Email}");
                Console.WriteLine($"{customerModel.Phone}");
                Console.WriteLine($"{customerModel.Address}");
                Console.WriteLine($"{customerModel.Zip}");
                Console.WriteLine($"{customerModel.Country}");

                //setting UID
                customerModel.UID = Guid.NewGuid();

                //Use the internal model object to create a DbContext object
                int createdEmployeeId = _customerRepository.Create(customerModel);

                //Map the DbContext object to a DTO object (removing potential personal or otherwise sensitive data)
                customerDto = _mapper.Map<CustomerGetDto>(customerModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Http POST exception: {ex.Message}");
                return StatusCode(500);
            }
            //For testing purpose
            if (customerCreateDto.FirstName.Equals("John") && customerCreateDto.FirstName.Equals("Doe"))
            {
                return StatusCode(500);
            }
            else
            {
                //Return information afbout URI (name=GetByID - HttpGet), Id for URI, and the created DTO
                return CreatedAtRoute(nameof(GetById), new { Id = customerDto.Id }, customerDto);
            }
        }
        /* HttpPut checks if an instance exists - if so, the instance is updated - otherwise a new instance is created*/
        [HttpPut]
        public IActionResult Put([FromBody] CustomerCreateDto customerCreateDto)
        {
            Console.WriteLine($"Http PUT recieved with data:");
            Console.WriteLine($"{customerCreateDto.Email}");

            Customer cust = _customerRepository.GetByEmail(customerCreateDto.Email);
            try
            {
                if (cust != null)
                {
                    Console.WriteLine($"Found customer based on email:" + customerCreateDto.Email);
                    //Map properties of src (first argument) to dest (second argument)
                    _mapper.Map(customerCreateDto, cust);
                    _customerRepository.Update(cust);
                }
                else
                {
                    var employeeModel = _mapper.Map<Customer>(customerCreateDto);
                    _customerRepository.Create(employeeModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CustomerControler - Put:" + ex.Message);
                return StatusCode(500);
            }
            return Ok();
        }
        [HttpDelete("{custId:int}")]
        public IActionResult Delete(int custId)
        {
            Console.WriteLine("CustomerController - Delete hit with ID:" + custId);
            Customer cust = _customerRepository.GetById(custId);
            if (cust != null)
            {
                _customerRepository.Delete(cust);
                return Ok();
            }
            return NotFound();
        }
    }
}
