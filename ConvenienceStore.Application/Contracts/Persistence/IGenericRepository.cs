using System.Linq.Expressions;

namespace ConvenienceStore.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, params string[] includes);
    Task<IReadOnlyList<T>> GetAllAsync(params string[] includes);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, params string[] includes);
    
    // Commands
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}
