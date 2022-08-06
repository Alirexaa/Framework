using Framework.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Persistence.EFCore.SqlServer.Internals;

internal class Repository<TEnity,TId> : IRepository<TEnity,TId> where TEnity : class
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEnity> _dbSet;
    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEnity>();
    }

    public async Task<TEnity> AddAsync(TEnity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbSet.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public async Task<int> DeleteAsync(Expression<Func<TEnity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var rowAffected = await _dbSet.Where(predicate).DeleteFromQueryAsync(cancellationToken);
        return rowAffected;
    }

    public async Task DeleteAsync(TEnity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.SingleDeleteAsync(entity, cancellationToken);
    }

    public async Task<int> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var rowAffected = await _dbSet.DeleteByKeyAsync(cancellationToken, id);
        return rowAffected;
        
    }

    public async Task<int> DeleteRangeAsync(IReadOnlyList<TEnity> entities, CancellationToken cancellationToken = default)
    {
        var rowAffected = await _dbSet.DeleteRangeByKeyAsync(cancellationToken, entities);
        return rowAffected;
    }

    public void Dispose()
    {
       
    }

    public async Task<IReadOnlyList<TEnity>> FindAsync(Expression<Func<TEnity, bool>> predicate, CancellationToken cancellationToken = default)
    {
       return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<TEnity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        var keys = new object[1] { id! };
        var entity = await _dbSet.FindAsync(keys, cancellationToken);
        return entity;
    }

    public async Task<TEnity?> FindOneAsync(Expression<Func<TEnity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(predicate).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEnity>> RawQuery(string query, CancellationToken cancellationToken = default, params object[] queryParams)
    {
        return await _dbSet.FromSqlRaw(query, queryParams).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEnity entity, CancellationToken cancellationToken = default)
    {
         await _dbSet.SingleUpdateAsync(entity, cancellationToken);
    }
}

