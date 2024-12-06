using System;
using System.Text;
using MakingOrder.Models;
using MakingOrder.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MakingOrder.Functions.API
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;

        public Function1(ILoggerFactory loggerFactory, IOrderService orderService)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _orderService = orderService;
        }

        [Function("Function1")]
        public void Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer)
        {
            string queueName = "email-send";
            //string userName = "guest"; // Default: "guest"
            //string password = "guest"; // Default: "guest"

            //// Create a connection factory
            //var factory = new ConnectionFactory
            //{
            //    UserName = userName,
            //    Password = password
            //};




            _logger.LogInformation("Running");
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection(); var channel = connection.CreateModel();
            channel.ExchangeDeclare("email-box", ExchangeType.Direct);
            channel.QueueDeclare("email-send", false, false, false);
            channel.QueueBind("email-send", "email-box", "email-add");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, e) =>
            {
                var body = e.Body;
                var json = System.Text.Encoding.UTF8.GetString(body.ToArray());
                var email = System.Text.Json.JsonSerializer.Deserialize<EmailInfo>(json);
                _logger.LogInformation($"Email sending executed at : {DateTime.Now}");
                _orderService.SendEmail(email.Body, email.Email);
            };
            channel.BasicConsume(queue: queueName,
                                    autoAck: false, // Set to true for auto-acknowledgment
                                    consumer: consumer);
            

            //_logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            //if (myTimer.ScheduleStatus is not null)
            //{
            //    _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            //}
        }
        public void test()
        {
            string rabbitMqHost = "your_rabbitmq_host";
            string queueName = "your_queue_name";
            string userName = "your_username"; // Default: "guest"
            string password = "your_password"; // Default: "guest"

            // Create a connection factory
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqHost,
                UserName = userName,
                Password = password
            };

            // Establish a connection and create a channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Ensure the queue exists (optional if you're sure it already exists)
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                Console.WriteLine($"Waiting for messages from queue: {queueName}");

                // Create a consumer
                var consumer = new EventingBasicConsumer(channel);

                // Event triggered when a message is received
                consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Received: {message}");

                    // Acknowledge the message
                    channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                };

                // Start consuming messages
                channel.BasicConsume(queue: queueName,
                                     autoAck: false, // Set to true for auto-acknowledgment
                                     consumer: consumer);

                Console.WriteLine("Press [Enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
