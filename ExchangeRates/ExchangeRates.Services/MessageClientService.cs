using ExchangeRates.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRates.Services
{
    public class MessageClientService
    {
        private readonly IOptions<MessageClientConfiguration> configuration;

        public MessageClientService(IOptions<MessageClientConfiguration> configuration)
        {
            this.configuration = configuration;
        }

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory { HostName = this.configuration.Value.Hostname };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: this.configuration.Value.QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var serializedMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: this.configuration.Value.QueueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
