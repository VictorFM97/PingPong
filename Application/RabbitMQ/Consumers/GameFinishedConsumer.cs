using Application.Extensions;
using Application.RabbitMQ.Events;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Application.RabbitMQ.Consumers;

public class GameFinishedConsumer : DefaultBasicConsumer
{
    private readonly IWinnersRepository WinnersRepository;

    public GameFinishedConsumer(IServiceProvider service)
    {
        WinnersRepository = service.GetRequiredService<IWinnersRepository>();
    }

    public async override void HandleBasicDeliver(
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

        try
        {
            var winner = await WinnersRepository.GetAsync(message.Winner);

            if(winner is null)
            {
                winner = new WinnerModel();
                winner.Winner = message.Winner;
                winner.Id = Guid.Empty;
            }

            winner.AmountOfTimesWon++;
            winner.GameId.Add(message.Id.ToString());

            await WinnersRepository.CreateOrUpdate(winner);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
