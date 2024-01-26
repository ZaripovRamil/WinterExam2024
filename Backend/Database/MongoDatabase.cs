using Contracts.Dbo;
using MongoDB.Driver;

namespace Database;

public interface IMongoDatabase
{
    Task<UserRatingDbo?> GetRatingAsync(Guid id);
    Task SetRatingAsync(UserRatingDbo userRating);
}

public class MongoDatabase : IMongoDatabase
{
    private readonly IMongoCollection<UserRatingDbo> _ratings;

    public MongoDatabase(IMongoClient mongoClient)
    {
        _ratings = mongoClient.GetDatabase("Ratings").GetCollection<UserRatingDbo>("Ratings");
    }

    public async Task<UserRatingDbo?> GetRatingAsync(Guid id)
    {
        return (await _ratings.FindAsync(rating => rating.UserId == id)).FirstOrDefault();
    }

    public async Task SetRatingAsync(UserRatingDbo userRating)
    {
        await _ratings.InsertOneAsync(userRating);
    }
}