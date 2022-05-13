using EmployeeService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishEmployee(EmployeePublishedDto employeePublishedDto);
        void PublishEmployees(List<EmployeePublishedDto> employeePublishedDtos);
    }
}
