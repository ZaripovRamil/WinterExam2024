namespace Contracts.Dto;

public class RoomDto
{
    public Guid Id { get; set; }
    public List<UserDto> Players { get; set; } = null!;
    public GameStateDto GameState { get; set; } = null!;
    public DateTime Created { get; set; }
    public int StatusCode { get; set; }
    public string OwnerName { get; set; } = null!;
}