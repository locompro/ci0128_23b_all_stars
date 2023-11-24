using Locompro.Common.Search;
using Locompro.Common.Search.QueryBuilder;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Data;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain;

/// <summary>
///     Generic domain service for an application entity type.
/// </summary>
/// <typeparam name="T">Type of entity handled by service.</typeparam>
/// <typeparam name="TK">Type of key used by entity.</typeparam>
public class DomainService<T, TK> : Service, IDomainService<T, TK>
    where T : class
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ICrudRepository<T, TK> CrudRepository;
    protected readonly IQueryBuilder QueryBuilder;

    /// <summary>
    ///     Constructs a domain service for a given repository.
    /// </summary>
    /// <param name="unitOfWork">Unit of work to handle transactions.</param>
    /// <param name="loggerFactory">Factory for service logger.</param>
    public DomainService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        UnitOfWork = unitOfWork;
        CrudRepository = UnitOfWork.GetCrudRepository<T, TK>();
        QueryBuilder = new QueryBuilder<T>(SearchMethodsFactory.GetInstance().Create<T>());
    }

    /// <inheritdoc />
    public async Task<T> Get(TK id)
    {
        return await CrudRepository.GetByIdAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetAll()
    {
        return await CrudRepository.GetAllAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetByDynamicQuery(List<ISearchCriterion> searchQueries)
    {
        foreach (var criterion in searchQueries)
        {
            try
            {
                QueryBuilder.AddSearchCriterion(criterion);
            }
            catch (ArgumentException exception)
            {
                // if the search criterion is invalid, report on it but continue execution
                Logger.LogWarning(exception.ToString());
            }
        }
        
        ISearchQueries builtQueries = QueryBuilder.GetSearchFunction();
        
        IEnumerable<T> results = builtQueries.IsEmpty()?
            null :
            await CrudRepository.GetByDynamicQuery(builtQueries);
        
        QueryBuilder.Reset();
        
        return results;
    }

    /// <inheritdoc />
    public async Task Add(T entity)
    {
        try
        {
            await CrudRepository.AddAsync(entity);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to add entity");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task Update(TK id, T entity)
    {
        try
        {
            await CrudRepository.UpdateAsync(id, entity);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to update entity");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task Delete(TK id)
    {
        try
        {
            await CrudRepository.DeleteAsync(id);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to delete entity");
            throw;
        }
    }
}