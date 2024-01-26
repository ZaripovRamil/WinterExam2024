using DatabaseServices.Repositories;

namespace WinterExam24.Services;

public class RoomCleaner : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public RoomCleaner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rooms = _serviceProvider.GetRequiredService<IRoomRepository>();
        while (true)
        {
            foreach (var room in await rooms.GetAll())
            {
                if (room.Players.Count == 0)
                {
                    await rooms.DeleteAsync(room);
                }
            }
            
            Thread.Sleep(60000);
        }
    }
}