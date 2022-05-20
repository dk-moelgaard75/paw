using TaskService.DTOs;
using TaskService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
