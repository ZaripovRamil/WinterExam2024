namespace Models;

public class Room
{
    public Guid Id { get; set; }
    public User[] Players { get; set; }
    public GameState GameState { get; set; }
}