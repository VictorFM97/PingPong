using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models;

public class WinnerModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public IList<string> GameId { get; set; }
    public string Winner { get; set; }
    public int AmountOfTimesWon { get; set; } = 0;

    public WinnerModel()
    {
        GameId = new List<string>();
    }
}
