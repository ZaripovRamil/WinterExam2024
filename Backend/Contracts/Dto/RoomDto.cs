namespace Contracts.Dto;

public class RoomDto
{
    public Guid Id { get; set; }
    public UserDto[] Players { get; set; }
    public GameStateDto GameState { get; set; }
    public DateTime Created { get; set; }
    public int StatusCode { get; set; }
}