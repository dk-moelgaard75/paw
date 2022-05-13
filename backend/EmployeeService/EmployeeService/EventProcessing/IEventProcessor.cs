using EmployeeService.DTOs;
using EmployeeService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.EventProcessing
{
    public interface IEventProcessor
    {
        //PawEnums.EventType DetermineEvent(CalenderRequestDto notifcationMessage);
        List<EmployeePublishedDto> GetEmployees(CalenderRequestDto message);
        EmployeePublishedDto GetEmployee(CalenderRequestDto message);
    }
}
