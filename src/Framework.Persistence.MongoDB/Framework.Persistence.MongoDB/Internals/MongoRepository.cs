using Framework.Abstraction.Types;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Persistence.MongoDB.Internals
{
    internal class MongoRepository<TEntity, TId> : IMongoRepository<TEntity, TId> where TEntity : IHaveIdentity<TId>
    {

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            Collection = database.GetCollection<TEntity>(collectionName);

        }

        public IMongoCollection<TEntity> Collection { get; }

        public async Task AddAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(TId id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            await DeleteAsync(e => e.Id!.Equals(id));
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await Collection.DeleteOneAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).AnyAsync();
        }

        public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public async Task<TEntity> GetAsync(TId id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            return await GetAsync(e => e.Id!.Equals(id));
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await UpdateAsync(entity, e => e.Id!.Equals(entity.Id));
        }

        public async Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            await Collection.ReplaceOneAsync(predicate, entity);
        }
    }

    internal class MongoRepository<TEntity> : MongoRepository<TEntity, long>, IMongoRepository<TEntity, long> where TEntity : IHaveIdentity<long>
    {
        public MongoRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {

        }
    }
}
