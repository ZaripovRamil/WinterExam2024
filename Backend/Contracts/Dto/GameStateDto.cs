using Common;

namespace Contracts.Dto;

public class GameStateDto
{
    public Dictionary<Guid, Move> Moves { get; set; }
    public Guid Winner { get; set; }
}