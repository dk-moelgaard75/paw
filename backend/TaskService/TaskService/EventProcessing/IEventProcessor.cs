using EmployeeService.DTOs;
using EmployeeService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskService.DTOs;

namespace TaskService.EventProcessing
{
    public interface IEventProcessor
    {
        //PawEnums.EventType DetermineEvent(CalenderRequestDto notifcationMessage);
        //List<EmployeePublishedDto> GetEmployees(CalenderRequestDto message);
        //EmployeePublishedDto GetEmployee(CalenderRequestDto message);
        List<TaskObjPublishedDto> GetTasks(CalenderRequestDto dto);
    }
}
