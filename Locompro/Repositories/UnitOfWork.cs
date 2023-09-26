using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Locompro.Repositories
{
    public class UnitOfWork
    {
        private readonly DbContext dbContext;
        private IDbContextTransaction transaction;

        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task BeginTransaction()
        {
            this.transaction = await this.dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            try
            {
                await this.dbContext.SaveChangesAsync();
                await this.transaction.CommitAsync();
            }
            catch
            {
                await Rollback();
                throw;
            }
            finally
            {
                if (this.transaction != null)
                {
                    await this.transaction.DisposeAsync();
                    this.transaction = null;
                }
            }
        }

        public async Task Rollback()
        {
            try
            {
                await this.transaction.RollbackAsync();
            }
            finally
            {
                if (this.transaction != null)
                {
                    await this.transaction.DisposeAsync();
                    this.transaction = null;
                }
            }
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().Wait();
        }

        private async ValueTask DisposeAsync()
        {
            if (this.transaction != null)
            {
                await this.transaction.DisposeAsync();
                this.transaction = null;
            }

            await this.dbContext.DisposeAsync();
        }
    }
}
