﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locompro.Data.Repositories;

/// <summary>
/// An application repository.
/// </summary>
/// <typeparam name="T">Type of entity handled by repository.</typeparam>
/// <typeparam name="TK">Type of key used by entity.</typeparam>
public interface ICrudRepository<T, TK> : IRepository
{
    /// <summary>
    /// Gets an entity based on its ID asynchronously.
    /// </summary>
    /// <param name="id">ID for the entity to return.</param>
    /// <returns>Entity for the passed ID.</returns>
    Task<T> GetByIdAsync(TK id);

    /// <summary>
    /// Gets all entities for this repository asynchronously.
    /// </summary>
    /// <returns>All entities for this repository.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Adds an entity to the repository asynchronously.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    Task AddAsync(T entity);

    /// <summary>
    /// Updates an entity for this repository asynchronously.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    void UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity for this repository asynchronously.
    /// </summary>
    /// <param name="id">ID for entity to delete.</param>
    Task DeleteAsync(TK id);
}