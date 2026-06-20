using System.Text;
using System.Text.Json;
using GoldenCrown.Contracts;
using RabbitMQ.Client;

namespace GoldenCrown.Messaging;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    public Task PublishAsync(TransactionCreatedEvent message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "tax-inspection",
            durable: false,
            exclusive: false,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message));

        channel.BasicPublish(
            exchange: "",
            routingKey: "tax-inspection",
            body: body);

        return Task.CompletedTask;
    }
}