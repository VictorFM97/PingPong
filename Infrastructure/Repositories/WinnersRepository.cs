using Infrastructure.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public interface IWinnersRepository
{
    Task<WinnerModel> GetAsync(string winner);
    Task CreateOrUpdate(WinnerModel model);
}

public class WinnersRepository : IWinnersRepository
{
    private readonly IMongoCollection<WinnerModel> Collection;

    public WinnersRepository(
        IOptions<WinnersDataBaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);

        var database = client.GetDatabase(settings.Value.DatabaseName);

        var databases = client.ListDatabases();

        Collection = database.GetCollection<WinnerModel>(settings.Value.CollectionName);
    }

    public async Task<WinnerModel> GetAsync(string winner)
    {
        return await Collection.Find(x => x.Winner == winner).FirstOrDefaultAsync();
    }

    public async Task CreateOrUpdate(WinnerModel model)
    {
        if (model.Id == Guid.Empty)
        {
            model.Id = Guid.NewGuid();
            await Collection.InsertOneAsync(model);
        }

        await Collection.ReplaceOneAsync(x => x.Id == model.Id, model);
    }
}
