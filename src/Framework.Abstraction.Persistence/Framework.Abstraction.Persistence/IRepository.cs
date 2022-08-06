namespace Framework.Abstraction.Persistence;
public interface IRepository<TEntity> : IRepository<TEntity, long>, IReadRepository<TEntity, long>, IWriteRepository<TEntity, long>, IDisposable where TEntity : class
{
}

public interface IRepository<TEntity, in TId> : IReadRepository<TEntity, TId>, IWriteRepository<TEntity, TId>, IDisposable where TEntity : class
{
}

