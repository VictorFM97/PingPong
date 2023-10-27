using Application.Extensions;
using Application.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Application.RabbitMQ;

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
        Console.WriteLine("Receiving Message");

        var message = JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(body.ToArray()));

        Console.WriteLine($"Message: {message.message}, force: {message.force}");

        Console.WriteLine("Finish Reading Message");

        Channel.SendMessage(Destinatary, new Message("hey!", Random.Next(0, 10)));

        Thread.Sleep(1000);
    }
}
