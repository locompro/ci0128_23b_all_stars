using Locompro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Locompro.Repositories
{
    /// <summary>
    /// Encapsulates a given DB transaction.
    /// </summary>
    public class UnitOfWork
    {
        private readonly LocomproContext dbContext;
        private IDbContextTransaction transaction;

        /// <summary>
        /// Constructs a unit of work for a given context.
        /// </summary>
        /// <param name="dbContext">Context to base unit of work on.</param>
        public UnitOfWork(LocomproContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Opens a DB transaction.
        /// </summary>
        public async Task BeginTransaction()
        {
            this.transaction = await this.dbContext.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commits a DB transaction.
        ///
        /// Rolls back transaction in the event of an exception.
        /// </summary>
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

        /// <summary>
        /// Rolls back a DB transaction.
        /// </summary>
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

        /// <summary>
        /// Disposes of an unused DB transaction.
        /// </summary>
        public void Dispose()
        {
            DisposeAsync().AsTask().Wait();
        }

        /// <summary>
        /// Disposes of an unused DB transaction asynchronously.
        /// </summary>
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
