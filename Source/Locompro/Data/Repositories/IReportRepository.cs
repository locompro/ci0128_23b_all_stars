using Locompro.Models.Entities;

namespace Locompro.Data.Repositories;

/// <inheritdoc />
public interface IReportRepository : ICrudRepository<Report, string>
{
    /// <summary>
    ///     Gets an entity based on its ID asynchronously.
    /// </summary>
    /// <param name="submissionUserId">First part of the primary key</param>
    /// <param name="submissionEntryTime">Second part of the primary key</param>
    /// <param name="userId">Third part of the primary key</param>
    /// <returns>Entity for the passed ID.</returns>
    Task<Report> GetByIdAsync(string submissionUserId, DateTime submissionEntryTime, string userId);

    /// <summary>
    /// Updates an entity for this repository asynchronously. If the entity does not exist, adds it.
    /// </summary>
    /// <param name="entity">The entity to add or update.</param>
    /// <param name="submissionUserId">First part of the primary key</param>
    /// <param name="submissionEntryTime">Second part of the primary key</param>
    /// <param name="userId">Third part of the primary key</param>
    /// <returns>The updated or added entity.</returns>
    Task UpdateAsync(string submissionUserId, DateTime submissionEntryTime, string userId, Report entity);

    /// <summary>
    /// Updates a list of entities for this repository asynchronously. If the entity does not exist, adds it.
    /// </summary>
    /// <param name="reports"> A list of reports to add</param>
    /// <returns>An async operation</returns>
    Task AddOrUpdateManyAutomaticReports(List<AutoReport> autoReports);
}