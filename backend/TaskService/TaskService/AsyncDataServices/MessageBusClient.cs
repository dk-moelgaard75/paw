using TaskService.Utils;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskService.DTOs;

namespace TaskService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private string RabbitMQEchangeString = "trigger";

        /*
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
                PawLogger.DoLog("TaskService - connected to RabbitMQ/MessageBus");

            }
            catch(Exception ex)
            {
                PawLogger.DoLog($"--> Could not connect to message bus: {ex.Message}");
            }
        }
        */
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            RabbitMqUtil.Initialize(_configuration);
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //
            PawLogger.DoLog("TaskService - RabbitMQ shutdown");
        }

        public void PublishNewTask(TaskObjPublishedDto taskObjPublishedDto)
        {
            var message = JsonSerializer.Serialize(taskObjPublishedDto);
            if (RabbitMqUtil.CalenderChannelOutgoing.IsOpen)
            {
                PawLogger.DoLog("TaskService - RabbitMQ connection is open - sending message");
                //TODO - send message
                SendMessage(message);
            }
            else
            {
                PawLogger.DoLog("TaskService - RabbitMQ connection is NOT open - NO message sent");
            }
        }
        public void PublishNewTask(List<TaskObjPublishedDto> taskObjPublishedDto)
        {
            var message = JsonSerializer.Serialize(taskObjPublishedDto);
            if (RabbitMqUtil.CalenderChannelOutgoing.IsOpen)
            {
                PawLogger.DoLog("TaskService - RabbitMQ connection is open - sending message:");
                PawLogger.DoLog(message);
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
            PawLogger.DoLog("RabbitMQExchange:" + _configuration["RabbitMQExchange"]);
            PawLogger.DoLog("RabbitMQRoutingKeyCalendarOutgoing:" + _configuration["RabbitMQRoutingKeyCalendarOutgoing"]);

            RabbitMqUtil.CalenderChannelOutgoing.BasicPublish(exchange: _configuration["RabbitMQExchange"],
                                                              routingKey: _configuration["RabbitMQRoutingKeyCalendarOutgoing"],
                                                                basicProperties: null,
                                                                body: messageBody);

            PawLogger.DoLog($"Taskservice - message sent thru RabbitMQ");
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
