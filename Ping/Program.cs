using Application.Extensions;
using Application.Models;
using Application.RabbitMQ;
using Application.RabbitMQ.Consumers;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

using var channel = ConnectionBuilder.GetConnection();
channel.CreateBasicProperties().Persistent = true;
channel.CreateQueue("ping");
channel.SendMessage("pong", new Message(5));

var pingPongConsumer = new PingPongConsumer(channel, "pong", new Random());

channel.BasicConsume(
    queue: "ping",
    autoAck: true,
    consumerTag: "",
    noLocal: true,
    exclusive: false,
    arguments: null,
    consumer: pingPongConsumer);

channel.ExchangeDeclare("events", ExchangeType.Direct, true);
channel.CreateQueue("notification", "events");
channel.CreateQueue("gameManager", "events");

channel.BasicConsume("gameManager", true, new GameFinishedConsumer());

app.Run();
