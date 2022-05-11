﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private string RabbitMQEchangeString = "trigger";

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: RabbitMQEchangeString, type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                PawLogger.DoLog("CalenddarService - connected to RabbitMQ/MessageBus");

            }
            catch (Exception ex)
            {
                PawLogger.DoLog(string.Format("Could not connect to message bus {0}", ex.Message));
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            PawLogger.DoLog("TaskService - RabbitMQ shutdown");
        }

        public void PublishNewTask(TaskObjPublishedDto taskObjPublishedDto)
        {
            var message = JsonSerializer.Serialize(taskObjPublishedDto);
            if (_connection.IsOpen)
            {
                PawLogger.DoLog("CalenddarService - RabbitMQ connection is open - sending message");
                //TODO - send message
                SendMessage(message);
            }
            else
            {
                PawLogger.DoLog("TaskService - RabbitMQ connection is NOT open - NO message sent");
            }
        }
        private void SendMessage(string message)
        {
            var messageBody = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: RabbitMQEchangeString,
                                    routingKey: "",
                                    basicProperties: null,
                                    body: messageBody);
            PawLogger.DoLog("Taskservice - message sent thru RabbitMQ");
        }
        public void Dispose()
        {
            PawLogger.DoLog("TaskService - MessageBus dispose");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
}