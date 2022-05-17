using EmployeeService.Utils;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.AsyncDataServices
{
    public class RabbitMqUtil
    {
        private static IConnection _connection;
        private static IModel _channelCalendarIncomming;
        private static IModel _channelCalendarOutgoing;
        private static string _calendarQueueNameIncomming;
        private static string _calendarQueueNameOutgoing;
        private static IConfiguration _configuration;
        private static bool _classInitialized = false;
        public static void Initialize(IConfiguration configuration)
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
                _channelCalendarIncomming = _connection.CreateModel();
                _channelCalendarOutgoing = _connection.CreateModel();

                //Declare Exchange - common for messages sent to both EmployeeService and TaskService
                _channelCalendarIncomming.ExchangeDeclare(exchange: _configuration["RabbitMQExchange"],
                                                type: ExchangeType.Direct);
                _channelCalendarOutgoing.ExchangeDeclare(exchange: _configuration["RabbitMQExchange"],
                                                type: ExchangeType.Direct);


                //Declare queue for sending/recieving employee data
                //Durable is true - if the server crashes, messages are persisted
                //Exclusive is false - other entities can alter the queue
                //Autodelete is false - the consume part should delete it
                _calendarQueueNameIncomming = _channelCalendarIncomming.QueueDeclare(
                                                _configuration["RabbitMQQueueCalendarIncomming"],
                                                false, 
                                                false, 
                                                false, 
                                                null).QueueName;

                _channelCalendarIncomming.QueueBind(_calendarQueueNameIncomming,
                                            _configuration["RabbitMQExchange"],
                                            _configuration["RabbitMQRoutingKeyCalendarIncomming"]);

                _calendarQueueNameOutgoing = _channelCalendarOutgoing.QueueDeclare(
                                                _configuration["RabbitMQQueueCalendarOutgoing"],
                                                false,
                                                false,
                                                false,
                                                null).QueueName;

                _channelCalendarOutgoing.QueueBind(_calendarQueueNameOutgoing,
                                            _configuration["RabbitMQExchange"],
                                            _configuration["RabbitMQRoutingKeyCalendarOutgoing"]);

                PawLogger.DoLog("EmployeeService - connected to RabbitMQ/MessageBus");
                _classInitialized = true;
            }
            catch (Exception ex)
            {
                PawLogger.DoLog(string.Format("Could not connect to message bus {0}", ex.Message));
                _classInitialized = false;
            }

        }
        public static bool IsInitialized
        {
            get
            {
                return _classInitialized;
            }
        }
        public static IConnection RabbitMQConnection
        {
            get
            {
                return _connection;
            }
        }
        public static IModel CalenderChannelIncomming
        {
            get
            {
                return _channelCalendarIncomming;
            }
        }
        public static IModel CalenderChannelOutgoing
        {
            get
            {
                return _channelCalendarOutgoing;
            }
        }
        public static string CalenderQueueNameIncomming
        {
            get
            {
                return _calendarQueueNameIncomming;
            }
        }
        public static string CalenderQueueNameOutgoing
        {
            get
            {
                return _calendarQueueNameOutgoing;
            }
        }

    }
}
