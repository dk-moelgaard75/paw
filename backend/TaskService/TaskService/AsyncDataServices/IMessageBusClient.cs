using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskService.DTOs;

namespace TaskService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewTask(TaskObjPublishedDto taskObjPublishedDto);
    }
}
