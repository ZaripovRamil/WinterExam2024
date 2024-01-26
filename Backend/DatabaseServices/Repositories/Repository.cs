
using Database;

namespace DatabaseServices.Repositories;

public interface IRepository<T>
{
    Task AddAsync(T room);
    Task<T?> GetAsync(Guid id);
    IEnumerable<T> GetAll();
    Task DeleteAsync(T room);
    Task UpdateAsync(T room);
}

public abstract class Repository
{
    protected AppDbContext DbContext { get; }

    protected Repository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }
}