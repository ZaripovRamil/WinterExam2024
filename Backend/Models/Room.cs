namespace Models;

public class Room
{
    public Guid Id { get; set; }
    public List<User> Players { get; set; } = new();
    public GameState GameState { get; set; } = new();
}