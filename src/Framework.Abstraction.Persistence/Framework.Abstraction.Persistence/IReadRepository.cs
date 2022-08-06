using System.Linq.Expressions;

namespace Framework.Abstraction.Persistence;

public interface IReadRepository<TEntity, in TId> where TEntity : class
{
    Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default(CancellationToken));

    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

    Task<IReadOnlyList<TEntity>> RawQuery(string query, CancellationToken cancellationToken = default(CancellationToken), params object[] queryParams);
}
