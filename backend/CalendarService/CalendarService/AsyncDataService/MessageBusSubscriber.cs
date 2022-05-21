using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client.Events;
using CalendarService.Utils;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using CalendarService.DTOs;
using CalendarService.Data;
using CalendarService.EventProcessing;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using CalendarService.Models;

namespace CalendarService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private IConfiguration _configuration;
        private IMessageBusClient _messageBusClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;


        public MessageBusSubscriber(IConfiguration configuration,
                                   IMessageBusClient msgBusClient,
                                    IServiceScopeFactory scopeFactory,
                                    IMapper mapper)
        {
            _configuration = configuration;
            RabbitMqUtil.Initialize(_configuration);
            _messageBusClient = msgBusClient;
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            PawLogger.DoLog("MessageBusSubscriber - calenderservice - init");

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            if (RabbitMqUtil.IsInitialized)
            {
                PawLogger.DoLog("MessageBusSubscriber - calenderservice - ExecuteAsync activated");
                var consumerEmployeeIncomming = new EventingBasicConsumer(RabbitMqUtil.EmployeeChannelIncomming);
                consumerEmployeeIncomming.Received += (ModuleHandle, ea) =>
                {
                    PawLogger.DoLog("CalendarService - MessageBusSubscribe/Employee - event received:");
                    var body = ea.Body;
                    var data = Encoding.UTF8.GetString(body.ToArray());
                    PawLogger.DoLog(data);
                    List<EmployeePublishDto> messages = JsonSerializer.Deserialize<List<EmployeePublishDto>>(data);
                    PawLogger.DoLog("####################################");
                    PawLogger.DoLog("EmployeeChannel");
                    PawLogger.DoLog("Nr of Recieved objects:" + messages.Count);
                    foreach (EmployeePublishDto dto in messages)
                    {
                        PawLogger.DoLog(dto.FirstName);
                        PawLogger.DoLog(dto.LastName);
                        PawLogger.DoLog(dto.Email);
                        PawLogger.DoLog(dto.CalendarGuid.ToString());
                    }

                    WriteEmployeesToDb(messages);
                };
                RabbitMqUtil.EmployeeChannelIncomming.BasicConsume(queue: RabbitMqUtil.EmployeeQueueNameIncomming, autoAck: true, consumer: consumerEmployeeIncomming);

                var consumerTaskIncomming = new EventingBasicConsumer(RabbitMqUtil.TaskChannelIncomming);
                consumerTaskIncomming.Received += (ModuleHandle, ea) =>
                {
                    PawLogger.DoLog("CalendarService - MessageBusSubscribe/Task - event received:");
                    var body = ea.Body;
                    var data = Encoding.UTF8.GetString(body.ToArray());
                    List<TaskObjPublishDto> messages = JsonSerializer.Deserialize<List<TaskObjPublishDto>>(data);
                    PawLogger.DoLog("####################################");
                    PawLogger.DoLog("TaskChannel");
                    PawLogger.DoLog("Nr of Recieved objects:" + messages.Count);
                    foreach(TaskObjPublishDto dto in messages)
                    {
                        PawLogger.DoLog(dto.CalendarGuid.ToString());
                        PawLogger.DoLog(dto.TaskGuid.ToString());
                        PawLogger.DoLog(dto.TaskName.ToString());
                        PawLogger.DoLog(dto.Employee.ToString());

                    }
                    WriteTasksToDb(messages);
                };
                RabbitMqUtil.TaskChannelIncomming.BasicConsume(queue: RabbitMqUtil.TaskQueueNameIncomming, autoAck: true, consumer: consumerTaskIncomming);
            }
            else
            {
                PawLogger.DoLog(" ERROR ERROR ERROR - RabbitMqUtil not initialized in CalendarService/MessageBusSubcriber");
            }
            return Task.CompletedTask;
        }

        private void WriteTasksToDb(List<TaskObjPublishDto> dtos)
        {
            Guid calenderGuid = Guid.Empty;
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICalendarRepository>();
                foreach(TaskObjPublishDto dto in dtos)
                {
                    if (calenderGuid == Guid.Empty)
                    {
                        calenderGuid = dto.CalendarGuid;
                    }
                    CalendarTaskObjModel model = _mapper.Map<CalendarTaskObjModel>(dto);
                    repo.CreateCalendarTaskObj(model);
                }
                CalendarModel calendarModel = repo.GetCalendarByCalendarGuid(calenderGuid);
                if (calendarModel != null)
                {
                    calendarModel.TaskDone = 1;
                    repo.UpdateCalendar(calendarModel);
                }

            }
        }
        private void WriteEmployeesToDb(List<EmployeePublishDto> dtos)
        {
            Guid calenderGuid = Guid.Empty;
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICalendarRepository>();
                foreach (EmployeePublishDto dto in dtos)
                {
                    if (calenderGuid == Guid.Empty)
                    {
                        calenderGuid = dto.CalendarGuid;
                    }
                    CalendarEmployeeModel model = _mapper.Map<CalendarEmployeeModel>(dto);
                    repo.CreateCalendarEmployee(model);
                }

                CalendarModel calendarModel = repo.GetCalendarByCalendarGuid(calenderGuid);
                if (calendarModel != null)
                {
                    calendarModel.EmployeeDone = 1;
                    repo.UpdateCalendar(calendarModel);
                }
            }
        }



        public override void Dispose()
        {
            bool closeConnection = false;
            if (RabbitMqUtil.EmployeeChannelIncomming.IsOpen)
            {
                RabbitMqUtil.EmployeeChannelIncomming.Close();
                closeConnection = true;
            }
            if (closeConnection)
            {
                RabbitMqUtil.RabbitMQConnection.Close();
            }
            base.Dispose();
        }

    }
}
