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

namespace CalendarService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private IConfiguration _configuration;
        private IMessageBusClient _messageBusClient;
        private IEventProcessor _eventProcessor;

        public MessageBusSubscriber(IConfiguration configuration, 
                                    ICalendarRepository calendarRepository,
                                    IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            RabbitMqUtil.Initialize(_configuration);
            _eventProcessor = eventProcessor;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            if (RabbitMqUtil.IsInitialized)
            {
                var consumerEmployeeIncomming = new EventingBasicConsumer(RabbitMqUtil.EmployeeChannelIncomming);
                consumerEmployeeIncomming.Received += (ModuleHandle, ea) =>
                {
                    PawLogger.DoLog("CalendarService - MessageBusSubscribe/Employee - event received:");
                    var body = ea.Body;
                    var data = Encoding.UTF8.GetString(body.ToArray());
                    PawLogger.DoLog(data);
                    List<EmployeePublishDto> list = JsonSerializer.Deserialize<List<EmployeePublishDto>>(data);
                    PawLogger.DoLog("####################################");
                    PawLogger.DoLog("EmployeeChannel");
                    PawLogger.DoLog("Nr of Recieved objects:" + list.Count);
                    foreach (EmployeePublishDto dto in list)
                    {
                        PawLogger.DoLog(dto.FirstName);
                        PawLogger.DoLog(dto.LastName);
                        PawLogger.DoLog(dto.Email);
                        PawLogger.DoLog(dto.CalendarGuid.ToString());
                    }
                };
                RabbitMqUtil.EmployeeChannelIncomming.BasicConsume(queue: RabbitMqUtil.EmployeeQueueNameIncomming, autoAck: true, consumer: consumerEmployeeIncomming);

                var consumerTaskIncomming = new EventingBasicConsumer(RabbitMqUtil.TaskChannelIncomming);
                consumerTaskIncomming.Received += (ModuleHandle, ea) =>
                {
                    PawLogger.DoLog("CalendarService - MessageBusSubscribe/Task - event received:");
                    var body = ea.Body;
                    var data = Encoding.UTF8.GetString(body.ToArray());
                    PawLogger.DoLog("####################################");
                    PawLogger.DoLog("EmployeeChannel");
                    PawLogger.DoLog(data);
                };
                RabbitMqUtil.EmployeeChannelIncomming.BasicConsume(queue: RabbitMqUtil.EmployeeQueueNameIncomming, autoAck: true, consumer: consumerTaskIncomming);
            }
            else
            {
                PawLogger.DoLog(" ERROR ERROR ERROR - RabbitMqUtil not initialized in CalendarService/MessageBusSubcriber");
            }
            return Task.CompletedTask;
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
