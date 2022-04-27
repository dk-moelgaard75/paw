using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.DTOs;
using CustomerService.Models;

namespace CustomerService.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            //Given that our Model object and DTO object have the
            //same propertynames there is no need for other than CreateMap
            //Remeber - automapper needs both "directions" - just because on map is set up, doesn´t
            //automatically create the reverse map

            //Source -> Target
            CreateMap<Customer, CustomerGetDto>();

            CreateMap<CustomerCreateDto, Customer>();
        }
    }
}
