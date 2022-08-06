using Framework.Abstraction.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.Persistence.EFCore.SqlServer.Internals
{
    internal class UnitOfWork<TApplicationDbContext> : IUnitOfWork  where TApplicationDbContext : DbContext
    {
        private readonly TApplicationDbContext _context;
        //private readonly IDbContextTransaction _dbContextTransaction;
        public UnitOfWork(TApplicationDbContext context)
        {
            ArgumentNullException.ThrowIfNull(_context, nameof(context));
            _context = context;
        }
        public void BeginTran()
        {
            _context.Database.BeginTransaction();
        }

        public async Task BeginTranAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public int CommitTran()
        {
            var rowEffect = _context.SaveChanges();
            _context.Database.CommitTransaction();
            return rowEffect;
        }

        public async Task<int> CommitTranAsync(CancellationToken cancellationToken = default)
        {
            var rowEffect = await _context.SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);
            return rowEffect;
        }

        public void RollBackTran()
        {
            _context.Database.RollbackTransaction();
        }

        public async Task RollBackTranAsync(CancellationToken cancellation = default)
        {
            await _context.Database.RollbackTransactionAsync(cancellation);
        }




        #region Dispose 
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion
    }
}
