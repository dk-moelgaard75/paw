using EmployeeService.DTOs;
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
        private IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private string RabbitMQEchangeString = "triggerX";
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
                System.Diagnostics.Debug.Print("RabbitMQPort:" + _configuration["RabbitMQPort"]);
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: RabbitMQEchangeString, type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("EmployeeService - connected to RabbitMQ/MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to message bus: {ex.Message}");
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //
            Console.WriteLine("EmployeeService - RabbitMQ shutdown");
        }
        public void PublishNewEmployee(EmployeePublishedDto employeePublishedDto)
        {
            var message = JsonSerializer.Serialize(employeePublishedDto);
            if (_connection.IsOpen)
            {
                System.Diagnostics.Debug.Print("EmployeeService - RabbitMQ connection is open - sending message");
                //TODO - send message
                SendMessage(message);
            }
            else
            {
                System.Diagnostics.Debug.Print("EmployeeService - RabbitMQ connection is NOT open - NO message sent");
            }

        }
        private void SendMessage(string message)
        {
            var messageBody = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: RabbitMQEchangeString,
                                    routingKey: "",
                                    basicProperties: null,
                                    body: messageBody);
            System.Diagnostics.Debug.Print($"EmployeeService - message sent thru RabbitMQ");
            
        }
        public void Dispose()
        {
            Console.WriteLine("EmployeeService - MessageBus dispose");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
