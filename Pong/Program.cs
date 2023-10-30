using Application.Extensions;
using Application.RabbitMQ;
using Application.RabbitMQ.Consumers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

using var channel = ConnectionBuilder.GetConnection();
channel.CreateBasicProperties().Persistent = true;
channel.CreateQueue("pong");

var pingPongConsumer = new PingPongConsumer(channel, "ping", new Random());

channel.BasicConsume(
    queue: "pong", 
    autoAck: true,
    consumerTag: "",
    noLocal: true,
    exclusive: false,
    arguments: null,
    consumer: pingPongConsumer);

app.Run();
