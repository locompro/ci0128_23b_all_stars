using Locompro.Data.Repositories;

namespace Locompro.Data;

public interface IUnitOfWork
{
    /// <summary>
    ///     Commits a DB transaction.
    ///     Rolls back transaction in the event of an exception.
    /// </summary>
    Task SaveChangesAsync();

    void RegisterRepository<TR>(TR repository) where TR : class, IRepository;

    ICrudRepository<T, TK> GetCrudRepository<T, TK>() where T : class;

    INamedEntityRepository<T, TK> GetNamedEntityRepository<T, TK>() where T : class;

    TR GetSpecialRepository<TR>() where TR : class, IRepository;
}