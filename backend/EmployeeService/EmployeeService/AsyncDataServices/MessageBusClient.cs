using EmployeeService.DTOs;
using EmployeeService.Utils;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        //This class is responsible for sending messages to the calendarservice
        private IConfiguration _configuration;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            RabbitMqUtil.Initialize(_configuration);
        }
        public void PublishEmployee(EmployeePublishedDto employeePublishedDto)
        {
            var message = JsonSerializer.Serialize(employeePublishedDto);
            if (RabbitMqUtil.CalenderChannelOutgoing.IsOpen)
            {
                PawLogger.DoLog("EmployeeService - RabbitMQ connection is open - sending message");
                //TODO - send message
                SendMessage(message);
            }
            else
            {
                PawLogger.DoLog("EmployeeService - RabbitMQ connection is NOT open - NO message sent");
            }

        }
        public void PublishEmployees(List<EmployeePublishedDto> employeePublishedDtos)
        {
            var message = JsonSerializer.Serialize(employeePublishedDtos);
            if (RabbitMqUtil.CalenderChannelOutgoing.IsOpen)
            {
                PawLogger.DoLog("EmployeeService - RabbitMQ connection is open - sending message");
                //TODO - send message
                SendMessage(message);
            }
            else
            {
                PawLogger.DoLog("EmployeeService - RabbitMQ connection is NOT open - NO message sent");
            }

        }
        private void SendMessage(string message)
        {
            var messageBody = Encoding.UTF8.GetBytes(message);
            PawLogger.DoLog("RabbitMQExchange:" + _configuration["RabbitMQExchange"]);
            PawLogger.DoLog("RabbitMQRoutingKeyCalendarOutgoing:" + _configuration["RabbitMQRoutingKeyCalendarOutgoing"]);
            RabbitMqUtil.CalenderChannelOutgoing.BasicPublish(exchange: _configuration["RabbitMQExchange"],
                                                        routingKey: _configuration["RabbitMQRoutingKeyCalendarOutgoing"],
                                                        basicProperties: null,
                                                        body: messageBody);


            PawLogger.DoLog("EmployeeService - message sent thru RabbitMQ");
            
        }
        public void Dispose()
        {
            PawLogger.DoLog("EmployeeService - MessageBus dispose");
            bool closeConnection = false;
            if (RabbitMqUtil.CalenderChannelOutgoing.IsOpen)
            {
                RabbitMqUtil.CalenderChannelOutgoing.Close();
                closeConnection = true;
            }
            if (RabbitMqUtil.CalenderChannelIncomming.IsOpen)
            {
                RabbitMqUtil.CalenderChannelIncomming.Close();
                closeConnection = true;
            }
            if (closeConnection)
            {
                RabbitMqUtil.RabbitMQConnection.Close();
            }
        }
    }
}
