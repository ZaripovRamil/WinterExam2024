using Common;

namespace Models;

public class GameState
{
    public Dictionary<string, Move> Moves { get; set; } = new();
    public string Winner { get; set; } = "";
}