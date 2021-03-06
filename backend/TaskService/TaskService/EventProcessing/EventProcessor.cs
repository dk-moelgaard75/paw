using AutoMapper;
using TaskService.Data;
using TaskService.DTOs;
using TaskService.Models;
using TaskService.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        /*
        public List<EmployeePublishedDto> GetEmployees(CalenderRequestDto message)
        {
            List<EmployeePublishedDto> list = GetEmployeesFromDB(message);
            return list;
        }
        public EmployeePublishedDto GetEmployee(CalenderRequestDto message)
        {
            List<EmployeePublishedDto> list = GetEmployeesFromDB(message);
            return list.First();
        }
        

        public PawEnums.EventType DetermineEvent(CalenderRequestDto notifcationMessage)
        {
            PawLogger.DoLog("EmployeeService Determining Event");

            if (notifcationMessage.SearchField.Equals("ALL") && notifcationMessage.SearchValue.Equals("ALL"))
            {
                return PawEnums.EventType.RequestAll;
            }
            else
            {
                return PawEnums.EventType.RequestSpecific;
            }
        }
        
        //This metode can handle both GetEmployee and GetEmployees based on the content of the message
        private List<EmployeePublishedDto> GetEmployeesFromDB(CalenderRequestDto notifcationMessage)
        {
            List<EmployeePublishedDto> list = new List<EmployeePublishedDto>();
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
                IEnumerable<Employee> employees = null;
                if (notifcationMessage.SearchField.Equals("ALL") && notifcationMessage.SearchValue.Equals("ALL"))
                {
                    employees = repo.GetAll();
                }
                else
                {
                    employees = repo.GetAll().Where(t => t.Email.Equals(notifcationMessage.SearchValue));
                }
                foreach (Employee emp in employees)
                {
                    EmployeePublishedDto dto = _mapper.Map<EmployeePublishedDto>(emp);
                    list.Add(dto);
                }
            }
            return list;
        }
        */

        public List<TaskObjPublishedDto> GetTasks(CalenderRequestDto dto)
        {
            List<TaskObjPublishedDto> newList = null;
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ITaskObjRepository>();
                IEnumerable<TaskObjPublishedDto> list = _mapper.Map<IEnumerable<TaskObjPublishedDto>>(repo.GetByStartDate(dto.StartDate));
                newList = list.ToList();
                foreach (TaskObjPublishedDto obj in newList)
                {
                    obj.CalendarGuid = Guid.Parse(dto.CalendarGuid);
                }
            }
            return newList;
        }
    }
}
