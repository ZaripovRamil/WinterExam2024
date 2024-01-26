using DatabaseServices.Repositories;

namespace WinterExam24.Services;

public class RoomCleaner : BackgroundService
{
    private readonly IRoomRepository _rooms;

    public RoomCleaner(IRoomRepository rooms)
    {
        _rooms = rooms;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            foreach (var room in await _rooms.GetAll())
            {
                if (room.Players.Count == 0)
                {
                    await _rooms.DeleteAsync(room);
                }
            }
            
            Thread.Sleep(60000);
        }
    }
}