using Application.Extensions;
using Application.Models;
using Application.RabbitMQ.Events;
using RabbitMQ.Client;

namespace Application.RabbitMQ.Consumers;

public class PingPongConsumer : DefaultBasicConsumer
{
    private readonly IModel Channel;
    private readonly string Destinatary;
    private readonly Random Random;

    public PingPongConsumer(IModel channel, string destinatary, Random random)
    {
        Channel = channel;
        Destinatary = destinatary;
        Random = random;
    }

    public override void HandleBasicDeliver(
        string consumerTag,
        ulong deliveryTag,
        bool redelivered,
        string exchange,
        string routingKey,
        IBasicProperties properties,
        ReadOnlyMemory<byte> body)
    {
        var message = body.DeserializeMessage<Message>();

        Console.WriteLine($"Number: {message.RNG}");

        if (message.RNG >= 9)
        {
            Channel.SendMessage(string.Empty, new GameFinishedEvent(Destinatary), "events");
        }

        Channel.SendMessage(Destinatary, new Message(Random.Next(0, 11)));

        Thread.Sleep(1000);
    }
}
