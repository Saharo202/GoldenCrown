using GoldenCrown.Contracts;

namespace GoldenCrown.Messaging;

public interface IRabbitMqPublisher
{
    Task PublishAsync(TransactionCreatedEvent message);
}