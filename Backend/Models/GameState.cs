using Common;

namespace Models;

public class GameState
{
    public Dictionary<User, Move> Moves { get; set; }
    public User Winner { get; set; }
}