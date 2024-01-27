using System.Text.Json;
using AutoMapper;
using Contracts.Dbo;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

    public Task<Room?> GetAsync(Guid id)
    {
        var dbo = RoomsWithIncludes().FirstOrDefault(dbo=> dbo.Id == id);
        var room = _mapper.Map<RoomDbo?, Room?>(dbo);
        if(room != null)
            room.Players = _mapper.Map<List<UserDbo>, List<User>>(dbo.Players);
        return Task.FromResult(room);
    }

    public Task<IEnumerable<Room>> GetAll()
    {
        return Task.FromResult(_mapper.Map<IEnumerable<RoomDbo>, IEnumerable<Room>>(RoomsWithIncludes()));
    }

    public async Task DeleteAsync(Room room)
    {
        var dbo = RoomsWithIncludes().FirstOrDefault(dbo=> dbo.Id == room.Id);
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
        var dbo = RoomsWithIncludes().FirstOrDefault(dbo=> dbo.Id == roomId);
        if (dbo is null) return;
        dbo.GameState = JsonSerializer.SerializeToDocument(gameState);
        await DbContext.SaveChangesAsync();
    }
    
    public async Task UpdatePlayers(Guid roomId, List<User> players)
    {
        var dbo = RoomsWithIncludes().FirstOrDefault(dbo=> dbo.Id == roomId);
        if (dbo is null) return;
        dbo.Players = players.Select(player => DbContext.Users.Find(player.Id)).ToList()!;
        await DbContext.SaveChangesAsync();
    }

    public IIncludableQueryable<RoomDbo, List<UserDbo>> RoomsWithIncludes()
    {
        return DbContext.Rooms.Include(r => r.Players);
    }
}