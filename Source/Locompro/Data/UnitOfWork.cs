using System.Collections.Concurrent;
using Locompro.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Locompro.Data
{
    /// <summary>
    /// DB transaction handler.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly bool _isTesting; // TODO: Move this func to testing impl
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, object> _repositories;

        private IDbContextTransaction _transaction;
        private readonly DbContext _context;

        /// <summary>
        /// Constructs a unit of work for a given context.
        /// </summary>
        /// <param name="loggerFactory">Logger for unit of work.</param>
        /// <param name="dataAccess">Context to base unit of work on.</param>
        public UnitOfWork(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, DbContext context)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _repositories = new ConcurrentDictionary<Type, object>();

            _serviceProvider = serviceProvider;
            _context = context;
            _isTesting = _context.Database.ProviderName.EndsWith("InMemory") ||
                         _context.Database.ProviderName.EndsWith("InMemoryDatabaseProvider");
        }

        /// <summary>
        /// Opens a DB transaction.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_isTesting) return;

            _transaction = await _context.Database.BeginTransactionAsync();
            _logger.LogInformation("Beginning transaction {}", _transaction.TransactionId);
        }

        /// <summary>
        /// Commits a DB transaction.
        ///
        /// Rolls back transaction in the event of an exception.
        /// </summary>
        public async Task CommitAsync()
        {
            if (_isTesting) return;
            
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                _logger.LogInformation("Committed transaction {}", _transaction.TransactionId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Commit failed for transaction {}", _transaction.TransactionId);
                await RollbackAsync();
                throw;
            }
            finally
            {
                await DisposeAsync();
            }
        }

        /// <summary>
        /// Rolls back a DB transaction.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_isTesting) return;
            
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await DisposeAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void RegisterRepository<TR>(TR repository) where TR : ICrudRepositoryBase
        {
            var type = typeof(TR);
            _repositories[type] = repository;
        }
        
        public ICrudRepository<T, I> GetRepository<T, I>() where T : class
        {
            return GetRepository<ICrudRepository<T, I>>();
        }

        public TR GetRepository<TR>() where TR : ICrudRepositoryBase
        {
            var type = typeof(TR);
            if (_repositories.TryGetValue(type, out var repository))
            {
                return (TR)repository;
            }

            // Use the service provider to resolve the repository.
            var newRepository = _serviceProvider.GetService<TR>();
            if (newRepository == null)
            {
                throw new InvalidOperationException($"Repository of type {type.FullName} is not registered.");
            }

            _repositories[type] = newRepository;
            return newRepository;
        }
    }
}