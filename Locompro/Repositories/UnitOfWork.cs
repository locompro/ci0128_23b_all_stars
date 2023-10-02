﻿using Locompro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Locompro.Repositories
{
    /// <summary>
    /// Encapsulates a given DB transaction.
    /// </summary>
    public class UnitOfWork
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly LocomproContext _dbContext;
        private IDbContextTransaction _transaction;

        /// <summary>
        /// Constructs a unit of work for a given context.
        /// </summary>
        /// <param name="logger">Logger for unit of work.</param>
        /// <param name="dbContext">Context to base unit of work on.</param>
        public UnitOfWork(ILogger<UnitOfWork> logger, LocomproContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Opens a DB transaction.
        /// </summary>
        public async Task BeginTransaction()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
            _logger.Log(LogLevel.Information, "Beginning transaction {}", _transaction.TransactionId);
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
                await _dbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch(Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Commit failed for transaction {}", _transaction.TransactionId);
                await Rollback();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
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
                await _transaction.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
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
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            await _dbContext.DisposeAsync();
        }
    }
}
