using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Dbo;

[PrimaryKey("Id")]
public class RoomDbo
{
    public Guid Id { get; set; }
    public List<UserDbo> Players { get; set; } = new();
    public JsonDocument GameState { get; set; }
    public DateTime Created { get; set; }
}