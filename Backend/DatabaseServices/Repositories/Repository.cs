
using Database;

namespace DatabaseServices.Repositories;

public interface IRepository<T>
{
    Task AddAsync(T item);
    Task<T?> GetAsync(Guid id);
    IEnumerable<T> GetAll();
    Task DeleteAsync(T item);
    Task UpdateAsync(T item);
}

public abstract class Repository
{
    protected AppDbContext DbContext { get; }

    protected Repository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }
}