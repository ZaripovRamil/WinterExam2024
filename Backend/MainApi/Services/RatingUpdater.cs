using Contracts.Dbo;
using Database;
using MassTransit;

namespace WinterExam24.Services;

public class RatingUpdater : IConsumer<UserRatingDbo>
{
    private readonly IMongoDatabase _mongoDatabase;

    public RatingUpdater(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public async Task Consume(ConsumeContext<UserRatingDbo> context)
    {
        await _mongoDatabase.SetRatingAsync(context.Message);
    }
}