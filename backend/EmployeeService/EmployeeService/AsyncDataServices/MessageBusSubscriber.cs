using EmployeeService.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using EmployeeService.DTOs;
using EmployeeService.EventProcessing;

namespace EmployeeService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private IConfiguration _configuration;
        private IMessageBusClient _messageBusClient;
        private IEventProcessor _eventProcessor;

        /*
private IConnection _connection;
private IModel _channel;
private string _queueName;
*/

        public MessageBusSubscriber(IConfiguration configuration, 
                                    IMessageBusClient msgBusClient,
                                    IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _messageBusClient = msgBusClient;
            _eventProcessor = eventProcessor;
            RabbitMqUtil.Initialize(_configuration);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            if (RabbitMqUtil.IsInitialized)
            {
                var consumer = new EventingBasicConsumer(RabbitMqUtil.CalenderChannelIncomming);
                consumer.Received += (ModuleHandle, ea) =>
                {
                    PawLogger.DoLog("EmployeeService - MessageBusSubscribe - event received:");
                    var body = ea.Body;
                    var data = Encoding.UTF8.GetString(body.ToArray());
                    PawLogger.DoLog("Data: " + data);
                    CalenderRequestDto message = JsonSerializer.Deserialize<CalenderRequestDto>(data);
                    PawLogger.DoLog("Message - searchField: " + message.SearchField);
                    PawLogger.DoLog("Message - searchValue: " + message.SearchValue);
                    PawLogger.DoLog("Message - calID: " + message.CalendarGuid);
                    _messageBusClient.PublishEmployees(_eventProcessor.GetEmployees(message));
                };
                RabbitMqUtil.CalenderChannelIncomming.BasicConsume(queue: RabbitMqUtil.CalenderQueueNameIncomming, autoAck: true, consumer: consumer);
            }

            return Task.CompletedTask;
        }

        
        /*
        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { 
                HostName = _configuration["RabbitMQHost"], 
                Port = int.Parse(_configuration["RabbitMQPort"]) 
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            
            _channel.ExchangeDeclare(exchange: _configuration["RabbitMQExchange"], type: ExchangeType.Fanout);
            
            _queueName = _channel.QueueDeclare().QueueName;
            
            _channel.QueueBind(queue: _queueName, exchange : _configuration["RabbitMQExchange"], routingKey: "");
            
            PawLogger.DoLog("EmployeeService - listning on the MessageBus");
            _connection.ConnectionShutdown += _connection_ConnectionShutdown;


        }
        */
        public override void Dispose()
        {
            bool closeConnection = false;
            if (RabbitMqUtil.CalenderChannelIncomming.IsOpen)
            {
                RabbitMqUtil.CalenderChannelIncomming.Close();
                closeConnection = true;
            }
            if (RabbitMqUtil.CalenderChannelOutgoing.IsOpen)
            {
                RabbitMqUtil.CalenderChannelOutgoing.Close();
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
