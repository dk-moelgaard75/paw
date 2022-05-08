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
                Console.WriteLine("TaskService - connected to RabbitMQ/MessageBus");

            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not connect to message bus: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //
            Console.WriteLine("TaskService - RabbitMQ shutdown");
        }

        public void PublishNewTask(TaskObjPublishedDto taskObjPublishedDto)
        {
            var message = JsonSerializer.Serialize(taskObjPublishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("TaskService - RabbitMQ connection is open - sending message");
                //TODO - send message
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("TaskService - RabbitMQ connection is NOT open - NO message sent");
            }
        }
        private void SendMessage(string message)
        {
            var messageBody = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: RabbitMQEchangeString,
                                    routingKey: "",
                                    basicProperties: null,
                                    body: messageBody);
            Console.WriteLine($"Taskservice - message sent thru RabbitMQ");
        }
        public void Dispose()
        {
            Console.WriteLine("TaskService - MessageBus dispose");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
