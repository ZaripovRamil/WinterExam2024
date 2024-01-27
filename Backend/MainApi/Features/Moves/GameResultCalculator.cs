using Common;
using Models;

namespace WinterExam24.Features.Moves;

public interface IGameResultCalculator
{
    public void UpdateRoomWithGameResult(Room room);
}
public class GameResultCalculator : IGameResultCalculator
{
    public void UpdateRoomWithGameResult(Room room)
    {
        KickQuitPlayers(room);
        FindWinner(room);
    }

    private static void FindWinner(Room room)
    {
        var moves = room.GameState.Moves.ToArray();
        if (moves[0].Value == moves[1].Value) return;
        var winningId = 0;
        if (moves[0].Value == Move.None)
        {
            winningId = 1;
        }
        else if (moves[1].Value == Move.None)
        {
            winningId = 0;
        }
        else if (moves[0].Value == Move.Paper)
        {
            winningId = moves[1].Value == Move.Rock ? 0 : 1;
        }
        if (moves[0].Value == Move.Rock)
        {
            winningId = moves[1].Value == Move.Scissors ? 0 : 1;
        }
        if (moves[0].Value == Move.Scissors)
        {
            winningId = moves[1].Value == Move.Paper ? 0 : 1;
        }

        room.GameState.Winner = moves[winningId].Key;
    }

    private static void KickQuitPlayers(Room room)
    {
        room.Players = room.Players.Where(player => room.GameState.Moves[player.UserName!] != Move.None).ToList();
        
    }
}