using AutoMapper;
using CustomerService.Data;
using CustomerService.DTOs;
using CustomerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCustomerController : ControllerBase
    {
        
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public GetCustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        [HttpGet("{id}", Name = "GetByGuid")]
        public ActionResult<CustomerGetDto> GetById(Guid id)
        {
            Console.WriteLine($"Http GET recieved with ID: {id}");
            Customer emp = _customerRepository.GetByGuid(id);
            if (emp != null)
            {
                return Ok(_mapper.Map<CustomerGetDto>(emp));
            }
            return NotFound();
        }

    }
}
