using Framework.Abstraction.Types;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Framework.Persistence.MongoDB;

public interface IMongoRepository<TEntity,TId> where TEntity : IHaveIdentity<TId>
{
    IMongoCollection<TEntity> Collection { get; }
    Task<TEntity> GetAsync(TId id);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate);
    Task DeleteAsync(TId id);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}

public interface IMongoRepository<TEntity> : IMongoRepository<TEntity,long> where TEntity : IHaveIdentity<long>
{

}
