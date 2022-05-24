using AutoMapper;
using EmployeeService.Data;
using EmployeeService.DTOs;
using EmployeeService.Models;
using EmployeeService.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

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
                PawLogger.DoLog("GetEmployeesFromDB");
                foreach (Employee emp in employees)
                {
                    PawLogger.DoLog("emp UID:" + emp.UID);
                    EmployeePublishedDto dto = _mapper.Map<EmployeePublishedDto>(emp);
                    PawLogger.DoLog("dto Guid:" + dto.EmployeeGuid);
                    dto.CalendarGuid = Guid.Parse(notifcationMessage.CalendarGuid);
                    list.Add(dto);
                }
            }
            return list;
        }
    }
}
