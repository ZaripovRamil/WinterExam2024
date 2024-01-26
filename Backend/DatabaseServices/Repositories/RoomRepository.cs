using System.Text.Json;
using AutoMapper;
using Contracts.Dbo;
using Database;
using Models;

namespace DatabaseServices.Repositories;

public interface IRoomRepository : IRepository<Room>
{
    public Task UpdateGameState(Guid roomId, GameState gameState);
    public Task UpdatePlayers(Guid roomId, List<User> players);
}

public class RoomRepository : Repository, IRoomRepository
{
    private readonly IMapper _mapper;
    public RoomRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public async Task AddAsync(Room room)
    {
        var dbo = _mapper.Map<Room, RoomDbo>(room);
        dbo.Players = room.Players.Select(player => DbContext.Users.Find(player.Id)).ToList()!;
        await DbContext.Rooms.AddAsync(dbo);
        await DbContext.SaveChangesAsync();
    }

    public async Task<Room?> GetAsync(Guid id)
    {
        var dbo = await DbContext.Rooms.FindAsync(id);
        return _mapper.Map<RoomDbo?, Room?>(dbo);
    }

    public Task<IEnumerable<Room>> GetAll()
    {
        return Task.FromResult(_mapper.Map<IEnumerable<RoomDbo>, IEnumerable<Room>>(DbContext.Rooms));
    }

    public async Task DeleteAsync(Room room)
    {
        var dbo = await DbContext.Rooms.FindAsync(room.Id);
        if(dbo is null) return;
        DbContext.Rooms.Remove(dbo);
        await DbContext.SaveChangesAsync();
    }

    public Task UpdateAsync(Room room)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateGameState(Guid roomId, GameState gameState)
    {
        var dbo = await DbContext.Rooms.FindAsync(roomId);
        if (dbo is null) return;
        dbo.GameState = JsonSerializer.SerializeToDocument(gameState);
        await DbContext.SaveChangesAsync();
    }
    
    public async Task UpdatePlayers(Guid roomId, List<User> players)
    {
        var dbo = await DbContext.Rooms.FindAsync(roomId);
        if (dbo is null) return;
        dbo.Players = players.Select(player => DbContext.Users.Find(player.Id)).ToList()!;
        await DbContext.SaveChangesAsync();
    }
}