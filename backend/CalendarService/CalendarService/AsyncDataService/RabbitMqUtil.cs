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
            string message = "";
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                message = "PAW:Creating connection";
                _connection = factory.CreateConnection();
                
                message = "PAW:Creating _channelEmployeeIncomming";
                _channelEmployeeIncomming = _connection.CreateModel();
                
                message = "PAW:Creating _channelEmployeeOutgoing";
                _channelEmployeeOutgoing = _connection.CreateModel();
                
                message = "PAW:Creating _channelTaskIncomming";
                _channelTaskIncomming = _connection.CreateModel();
                
                message = "PAW:Creating _channelTaskOutgoing";
                _channelTaskOutgoing = _connection.CreateModel();

                //Declare Exchange - common for messages sent to both EmployeeService and TaskService
                message = "PAW:Creating ExchangeDeclare (incom):" + _configuration["RabbitMQExchange"];
                _channelEmployeeIncomming.ExchangeDeclare(exchange: _configuration["RabbitMQExchange"],
                                                type: ExchangeType.Direct);
                
                message = "PAW:Creating ExchangeDeclare (outgo):" + _configuration["RabbitMQExchange"];
                _channelEmployeeOutgoing.ExchangeDeclare(exchange: _configuration["RabbitMQExchange"],
                                                type: ExchangeType.Direct);


                //Declare queue for sending/recieving employee data
                //Durable is false - if the message is lost the user will try again
                //Exclusive is false - other entities can alter the queue
                //Autodelete is true - no need for messages to be deleted programmatically
                message = "PAW:Creating _employeeQueueNameIncomming:" + _configuration["RabbitMQQueueEmployeeIncomming"];
                _employeeQueueNameIncomming = _channelEmployeeIncomming.QueueDeclare(
                                                _configuration["RabbitMQQueueEmployeeIncomming"], 
                                                false, 
                                                false,
                                                false, 
                                                null).QueueName;

                message = "PAW:Creating _channelEmployeeIncomming (QueueBind):" + _configuration["RabbitMQExchange"] + "/" + _configuration["RabbitMQRoutingKeyEmployeeIncomming"];
                _channelEmployeeIncomming.QueueBind(_employeeQueueNameIncomming,
                                            _configuration["RabbitMQExchange"],
                                            _configuration["RabbitMQRoutingKeyEmployeeIncomming"]);

                message = "PAW:Creating _channelEmployeeOutgoing (QueueDeclare):" + _configuration["RabbitMQQueueEmployeeOutgoing"];
                _employeeQueueNameOutgoing = _channelEmployeeOutgoing.QueueDeclare(
                                                _configuration["RabbitMQQueueEmployeeOutgoing"],
                                                false,
                                                false,
                                                false,
                                                null).QueueName;
                
                message = "PAW:Creating _channelEmployeeOutgoing (QueueBind):" + _configuration["RabbitMQExchange"] + "/" + _configuration["RabbitMQRoutingKeyEmployeeOutgoing"];
                _channelEmployeeOutgoing.QueueBind(_employeeQueueNameOutgoing,
                                        _configuration["RabbitMQExchange"],
                                        _configuration["RabbitMQRoutingKeyEmployeeOutgoing"]);

                /*****************************************************************************/
                
                message = "PAW:Creating _taskQueueNameIncomming:" + _configuration["RabbitMQQueueTaskIncomming"];
                _taskQueueNameIncomming = _channelTaskIncomming.QueueDeclare(
                                                _configuration["RabbitMQQueueTaskIncomming"],
                                                false,
                                                false,
                                                false,
                                                null).QueueName;
                
                message = "PAW:Creating _channelTaskIncomming (QueueBind):" + _configuration["RabbitMQExchange"] + "/" + _configuration["RabbitMQRoutingKeyTaskIncomming"];
                _channelTaskIncomming.QueueBind(_taskQueueNameIncomming,
                                        _configuration["RabbitMQExchange"],
                                        _configuration["RabbitMQRoutingKeyTaskIncomming"]);


                message = "PAW:Creating _taskQueueNameOutgoing (QueueDeclare):" + _configuration["RabbitMQQueueTaskOutgoing"];
                _taskQueueNameOutgoing = _channelTaskOutgoing.QueueDeclare(
                                                _configuration["RabbitMQQueueTaskOutgoing"],
                                                false,
                                                false,
                                                false,
                                                null).QueueName;

                message = "PAW:Creating _channelTaskOutgoing (QueueBind):" + _configuration["RabbitMQExchange"] + "/" + _configuration["RabbitMQRoutingKeyTaskOutgoing"];
                _channelTaskOutgoing.QueueBind(_taskQueueNameOutgoing,
                                        _configuration["RabbitMQExchange"],
                                        _configuration["RabbitMQRoutingKeyTaskOutgoing"]);

                PawLogger.DoLog("CalenddarService - connected to RabbitMQ/MessageBus");
                _classInitialized = true;
            }
            catch (Exception ex)
            {
                PawLogger.DoLog(string.Format("Could not connect to message bus {0}{1}", ex.Message, message));
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

