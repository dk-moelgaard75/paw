using AutoMapper;
using EmployeeService.DTOs;
using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            //Given that our Model object and DTO object have the
            //same propertynames there is no need for other than CreateMap
            //Remeber - automapper needs both "directions" - just because on map is set up, doesn´t
            //automatically create the reverse map

            //Source -> Target
            CreateMap<Employee, EmployeeGetDto>();
            
            CreateMap<EmployeeCreateDto, Employee>();
            //CreateMap<EmployeeCreateDto, Employee>();

            //Map employee and handle UID af EmployeGuid in the process
            CreateMap<Employee, EmployeePublishedDto>()
                .ForMember(dest => dest.EmployeeGuid, opt => opt.MapFrom(src => src.UID));
        }   
    }
}
