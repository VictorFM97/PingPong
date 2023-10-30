using Application.Extensions;
using Application.RabbitMQ;
using Application.RabbitMQ.Consumers;
using Infrastructure;
using Infrastructure.Repositories;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WinnersDataBaseSettings>(
    builder.Configuration.GetSection("WinnersDataBaseSettings"));

builder.Services.AddSingleton<IWinnersRepository, WinnersRepository>();

var app = builder.Build();

using var channel = ConnectionBuilder.GetConnection();
channel.ExchangeDeclare("events", ExchangeType.Direct, true);
channel.CreateQueue("notification", "events");
channel.CreateQueue("gameManager", "events");

channel.BasicConsume("gameManager", true, new GameFinishedConsumer(app.Services));

app.Run();