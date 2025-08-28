using System.Linq.Expressions;

namespace Daveslist.Domain.Shared.Interfaces.Repositories;

public interface IBaseRepository<T, TKey>
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
