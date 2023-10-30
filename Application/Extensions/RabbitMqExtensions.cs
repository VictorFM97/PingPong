using Application.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Application.Extensions;

public static class RabbitMqExtensions
{
    public static void SendMessage<T>(this IModel model, string routingKey, T message, string exchange = "")
        where T : class
    {
        var json = JsonSerializer.Serialize(message);
        model.BasicPublish(
            exchange, 
            routingKey,  
            body: Encoding.UTF8.GetBytes(json));
    }

    public static void CreateQueue(this IModel model, string queueName, string exchangeName = null)
    {
        model.QueueDeclare(
            queueName, 
            durable: true,
            exclusive: false,
            autoDelete: false);

        if(exchangeName != null)
        {
            model.QueueBind(queueName, exchangeName, string.Empty);
        }
    }

    public static T DeserializeMessage<T>(this ReadOnlyMemory<byte> bytes)
        where T : class => JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes.ToArray()));

}
