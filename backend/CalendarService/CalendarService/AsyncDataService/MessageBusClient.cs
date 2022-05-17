using CalendarService.DTOs;
using CalendarService.Utils;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalendarService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        
        private IConfiguration _configuration;
        /*
        private IConnection _connection;
        private IModel _channelEmployee;
        private IModel _channelTask;
        */
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            RabbitMqUtil.Initialize(configuration);
        }

        public void RequestEmployee(string searchField, string searchValue)
        {
            if (RabbitMqUtil.IsInitialized)
            {
                EmployeeGetDto empGetDto = new EmployeeGetDto() { SearchField = searchField, SearchValue = searchValue };
                var message = JsonSerializer.Serialize(empGetDto);
                //if (_connection.IsOpen)
                if (RabbitMqUtil.RabbitMQConnection.IsOpen)
                {
                    PawLogger.DoLog(String.Format("CalenddarService - RabbitMQ connection is open - requesting empoyee ({0}/{1}", searchField, searchValue));
                    //TODO - send message
                    SendEmployeeMessage(message);
                }
                else
                {
                    PawLogger.DoLog("TaskService - RabbitMQ connection is NOT open - NO message sent");
                }
            }
            else
            {
                PawLogger.DoLog("TaskService - RabbitMQUtil not initialized");
            }
        }

        public void RequestEmployees(string calendarGuid)
        {
            EmployeeGetDto empGetDto = new EmployeeGetDto() { SearchField = "ALL", SearchValue = "ALL", CalendarGuid = calendarGuid };
            var message = JsonSerializer.Serialize(empGetDto);
            if (RabbitMqUtil.RabbitMQConnection.IsOpen)
            {
                PawLogger.DoLog("CalenddarService - RabbitMQ connection is open - requesting all empoyees");
                SendEmployeeMessage(message);
            }
            else
            {
                PawLogger.DoLog("TaskService - RabbitMQ connection is NOT open - NO message sent");
            }
        }
        private void SendEmployeeMessage(string message)
        {
            var messageBody = Encoding.UTF8.GetBytes(message);
            RabbitMqUtil.EmployeeChannelOutgoing.BasicPublish(exchange: _configuration["RabbitMQExchange"],
                                                      routingKey: _configuration["RabbitMQRoutingKeyEmployeeOutgoing"],
                                                        basicProperties:  null,
                                                        body: messageBody);
            PawLogger.DoLog("Taskservice - SendEmployeeMessage message - sent thru RabbitMQ");
        }

        public void RequestTasks()
        {
            throw new NotImplementedException();
        }

        public void RequestTask(DateTime startDate, string calendarGuid)
        {
            TaskObjGetDto taskGetDto = new TaskObjGetDto() { StartDate = startDate, CalendarGuid = calendarGuid };
            var message = JsonSerializer.Serialize(taskGetDto);
            if (RabbitMqUtil.RabbitMQConnection.IsOpen)
            {
                PawLogger.DoLog("CalenddarService - RequestTask - RabbitMQ connection is open - requesting all tasks after:" + startDate.ToString("d"));
                SendTaskMessage(message);
            }
            else
            {
                PawLogger.DoLog("TaskService - RabbitMQ connection is NOT open - NO message sent");
            }

        }
        private void SendTaskMessage(string message)
        {
            var messageBody = Encoding.UTF8.GetBytes(message);
            RabbitMqUtil.EmployeeChannelOutgoing.BasicPublish(exchange: _configuration["RabbitMQExchange"],
                                                      routingKey: _configuration["RabbitMQRoutingKeyTaskOutgoing"], 
                                                        basicProperties: null,
                                                        body: messageBody);
            PawLogger.DoLog("Taskservice - SendTaskMessage - message sent thru RabbitMQ");
        }


        public void Dispose()
        {
            PawLogger.DoLog("TaskService - MessageBus dispose");
            bool closeConnection = false;
            if (RabbitMqUtil.EmployeeChannelIncomming.IsOpen)
            {
                RabbitMqUtil.EmployeeChannelIncomming.Close();
                closeConnection = true;
            }
            if (RabbitMqUtil.EmployeeChannelOutgoing.IsOpen)
            {
                RabbitMqUtil.EmployeeChannelOutgoing.Close();
                closeConnection = true;
            }
            if (RabbitMqUtil.TaskChannelIncomming.IsOpen)
            {
                RabbitMqUtil.EmployeeChannelIncomming.Close();
                closeConnection = true;
            }
            if (RabbitMqUtil.TaskChannelOutgoing.IsOpen)
            {
                RabbitMqUtil.EmployeeChannelIncomming.Close();
                closeConnection = true;
            }

            if (closeConnection)
            {
                RabbitMqUtil.RabbitMQConnection.Close();
            }
               
        }

    }
}
