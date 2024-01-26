using Database;
using Models;

namespace DatabaseServices.Repositories;

public interface IRoomRepository : IRepository<Room>
{
}
public class RoomRepository : Repository, IRoomRepository
{
    public RoomRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public Task AddAsync(Room item)
    {
        throw new NotImplementedException();
    }

    public Task<Room?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Room> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Room item)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Room item)
    {
        throw new NotImplementedException();
    }
}