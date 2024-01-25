using System.Text.Json;

namespace Contracts.Dbo;

public class RoomDbo
{
    public Guid Id { get; set; }
    public UserDbo[] Players { get; set; }
    public JsonDocument GameState { get; set; }
}