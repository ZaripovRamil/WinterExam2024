using Common;

namespace Contracts.Dto;

public class GameStateDto
{
    public Dictionary<string, Move> Moves { get; set; }
    public string Winner { get; set; }
}