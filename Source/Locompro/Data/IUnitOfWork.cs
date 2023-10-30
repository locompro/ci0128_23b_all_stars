using Locompro.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;

public interface IUnitOfWork
{
    /// <summary>
    /// Opens a DB transaction.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits a DB transaction.
    ///
    /// Rolls back transaction in the event of an exception.
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Rolls back a DB transaction.
    /// </summary>
    Task RollbackAsync();

    /// <summary>
    /// Disposes of a DB transaction.
    /// </summary>
    ValueTask DisposeAsync();

    void RegisterRepository<TR>(TR repository) where TR : ICrudRepositoryBase;
    
    ICrudRepository<T, I> GetRepository<T, I>() where T : class;
    
    TR GetRepository<TR>()  where TR : ICrudRepositoryBase;
}