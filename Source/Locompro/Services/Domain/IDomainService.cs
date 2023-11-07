namespace Locompro.Services.Domain;

/// <summary>
///     An application domain service.
/// </summary>
/// <typeparam name="T">Type of entity handled by service.</typeparam>
/// <typeparam name="TK">Type of key used by entity.</typeparam>
public interface IDomainService<T, TK> where T : class
{
    /// <summary>
    ///     Gets an entity through this service based on its ID.
    /// </summary>
    /// <param name="id">ID for the entity to return.</param>
    /// <returns>Entity for the passed ID.</returns>
    Task<T> Get(TK id);

    /// <summary>
    ///     Gets all entities through this service.
    /// </summary>
    /// <returns>All entities for this service.</returns>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    ///     Adds an entity through the service.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    Task Add(T entity);

    /// <summary>
    ///     Updates an entity through this service. If it does not exist, adds it.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity">Entity to update.</param>
    Task Update(TK id, T entity);

    /// <summary>
    ///     Deletes an entity through this service based on its ID.
    /// </summary>
    /// <param name="id">ID for entity to delete.</param>
    Task Delete(TK id);
}