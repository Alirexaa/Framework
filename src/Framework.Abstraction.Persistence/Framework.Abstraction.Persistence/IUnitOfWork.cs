namespace Framework.Abstraction.Persistence;

public interface IUnitOfWork : IDisposable
{
    void BeginTran();
    Task BeginTranAsync(CancellationToken cancellationToken = default);
    int CommitTran();
    Task<int> CommitTranAsync(CancellationToken cancellationToken = default);
    void RollBackTran();
    Task RollBackTranAsync(CancellationToken cancellation = default);
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}

public interface IUnitOfWork<out TContext> : IUnitOfWork, IDisposable where TContext : class
{
    TContext Context { get; }
}
