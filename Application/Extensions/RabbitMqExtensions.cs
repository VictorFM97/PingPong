using Application.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Application.Extensions;

public static class RabbitMqExtensions
{
    public static void SendMessage(this IModel model, string destinatary, Message message)
    {
        var json = JsonSerializer.Serialize(message);
        model.BasicPublish(
            "", 
            destinatary,  
            body: Encoding.UTF8.GetBytes(json));
    }

    public static void CreateQueue(this IModel model, string queueName)
    {
        model.QueueDeclare(
            queueName, 
            durable: true,
            exclusive: false,
            autoDelete: false);
    }
}
