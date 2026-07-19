using System.Linq.Expressions;
using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConvenienceStore.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<T?> GetByIdAsync(int id, params string[] includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        var entityType = _dbContext.Model.FindEntityType(typeof(T));
        var primaryKey = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();
        if (primaryKey != null && includes != null && includes.Length > 0)
        {
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKey.Name) == id);
        }
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(params string[] includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, params string[] includes)
    {
        IQueryable<T> query = _dbContext.Set<T>().Where(predicate);
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
    }
}
