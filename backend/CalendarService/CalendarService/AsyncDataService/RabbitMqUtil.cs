using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalendarService.Utils;

namespace CalendarService.AsyncDataService
{
    public static class RabbitMqUtil
    {
        private static IConnection _connection;
        private static IModel _channelEmployeeIncomming;
        private static IModel _channelEmployeeOutgoing;
        private static IModel _channelTaskIncomming;
        private static IModel _channelTaskOutgoing;
        private static string _employeeQueueNameIncomming;
        private static string _employeeQueueNameOutgoing;
        private static string _taskQueueNameIncomming;
        private static string _taskQueueNameOutgoing;
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
                _channelEmployeeIncomming = _connection.CreateModel();
                _channelEmployeeOutgoing = _connection.CreateModel();
                _channelTaskIncomming = _connection.CreateModel();
                _channelTaskOutgoing = _connection.CreateModel();

                //Declare Exchange - common for messages sent to both EmployeeService and TaskService
                _channelEmployeeIncomming.ExchangeDeclare(exchange: _configuration["RabbitMQExchange"],
                                                type: ExchangeType.Direct);
                _channelEmployeeOutgoing.ExchangeDeclare(exchange: _configuration["RabbitMQExchange"],
                                                type: ExchangeType.Direct);


                //Declare queue for sending/recieving employee data
                //Durable is false - if the message is lost the user will try again
                //Exclusive is false - other entities can alter the queue
                //Autodelete is true - no need for messages to be deleted programmatically
                _employeeQueueNameIncomming = _channelEmployeeIncomming.QueueDeclare(
                                                _configuration["RabbitMQQueueEmployeeIncomming"], 
                                                false, 
                                                false,
                                                false, 
                                                null).QueueName;

                _channelEmployeeIncomming.QueueBind(_employeeQueueNameIncomming,
                                            _configuration["RabbitMQExchange"],
                                            _configuration["RabbitMQRoutingKeyEmployeeIncomming"]);

                _employeeQueueNameOutgoing = _channelEmployeeOutgoing.QueueDeclare(
                                                _configuration["RabbitMQQueueEmployeeOutgoing"],
                                                false,
                                                false,
                                                false,
                                                null).QueueName;

                _channelEmployeeOutgoing.QueueBind(_employeeQueueNameOutgoing,
                                        _configuration["RabbitMQExchange"],
                                        _configuration["RabbitMQRoutingKeyEmployeeOutgoing"]);

                /*****************************************************************************/

                _taskQueueNameIncomming = _channelTaskIncomming.QueueDeclare(
                                                _configuration["RabbitMQQueueTaskIncomming"],
                                                false,
                                                false,
                                                false,
                                                null).QueueName;

                _channelTaskIncomming.QueueBind(_taskQueueNameIncomming,
                                        _configuration["RabbitMQExchange"],
                                        _configuration["RabbitMQRoutingKeyTaskIncomming"]);


                _taskQueueNameOutgoing = _channelTaskOutgoing.QueueDeclare(
                                                _configuration["RabbitMQQueueTaskOutgoing"],
                                                false,
                                                false,
                                                false,
                                                null).QueueName;

                _channelTaskOutgoing.QueueBind(_taskQueueNameOutgoing,
                                        _configuration["RabbitMQExchange"],
                                        _configuration["RabbitMQRoutingKeyTaskOutgoing"]);

                PawLogger.DoLog("CalenddarService - connected to RabbitMQ/MessageBus");
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
        public static IModel EmployeeChannelIncomming
        {
            get
            {
                return _channelEmployeeOutgoing;
            }
        }
        public static IModel EmployeeChannelOutgoing
        {
            get
            {
                return _channelEmployeeOutgoing;
            }
        }
        public static IModel TaskChannelIncomming
        {
            get
            {
                return _channelTaskIncomming;
            }
        }
        public static IModel TaskChannelOutgoing
        {
            get
            {
                return _channelTaskOutgoing;
            }
        }
        public static string EmployeeQueueNameIncomming
        {
            get
            {
                return _employeeQueueNameIncomming;
            }
        }
        public static string EmployeeQueueNameOutgoing
        {
            get
            {
                return _employeeQueueNameOutgoing;
            }
        }
        public static string TaskQueueNameIncomming
        {
            get
            {
                return _taskQueueNameIncomming;
            }
        }
        public static string TaskQueueNameOutgoing
        {
            get
            {
                return _taskQueueNameIncomming;
            }
        }

    }
}

