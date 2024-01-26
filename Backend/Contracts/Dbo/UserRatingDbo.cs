using MongoDB.Bson;

namespace Contracts.Dbo;

public class UserRatingDbo
{
    public ObjectId Id { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }
}