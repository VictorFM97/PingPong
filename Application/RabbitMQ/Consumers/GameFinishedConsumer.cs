using Application.Extensions;
using Application.RabbitMQ.Events;
using RabbitMQ.Client;

namespace Application.RabbitMQ.Consumers;

public class GameFinishedConsumer : DefaultBasicConsumer
{
    public override void HandleBasicDeliver(
        string consumerTag,
        ulong deliveryTag,
        bool redelivered,
        string exchange,
        string routingKey,
        IBasicProperties properties,
        ReadOnlyMemory<byte> body)
    {
        var message = body.DeserializeMessage<GameFinishedEvent>();
        
        Console.WriteLine($"Winner: {message.Winner}, Guid = {message.Id}");
    }
}
