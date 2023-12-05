using Locompro.Common.Search;

namespace Locompro.Data.Repositories;

/// <summary>
///     An application repository.
/// </summary>
/// <typeparam name="T">Type of entity handled by repository.</typeparam>
/// <typeparam name="TK">Type of key used by entity.</typeparam>
public interface ICrudRepository<T, TK> : IRepository
{
    /// <summary>
    ///     Gets an entity based on its ID asynchronously.
    /// </summary>
    /// <param name="id">ID for the entity to return.</param>
    /// <returns>Entity for the passed ID.</returns>
    Task<T> GetByIdAsync(TK id);

    /// <summary>
    ///     Gets all entities for this repository asynchronously.
    /// </summary>
    /// <returns>All entities for this repository.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    ///     Adds an entity to the repository asynchronously.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    Task AddAsync(T entity);

    /// <summary>
    ///     Updates an entity for this repository asynchronously. If the entity does not exist, adds it.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity">Entity to update.</param>
    Task UpdateAsync(TK id, T entity);
    
    /// <summary>
    ///     Deletes an entity for this repository asynchronously.
    /// </summary>
    /// <param name="id">ID for entity to delete.</param>
    Task DeleteAsync(TK id);
    
    /// <summary>
    /// Gets the search results of the entity type according to the list of search criteria or queries to be used
    /// </summary>
    /// <param name="searchQueries"> search queries, criteria or strategies to be used to find the desired submissions</param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetByDynamicQuery(ISearchQueries<T> searchQueries);
}