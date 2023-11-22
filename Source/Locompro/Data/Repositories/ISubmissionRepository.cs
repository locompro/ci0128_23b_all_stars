using Locompro.Common.Search;
using Locompro.Models.Entities;

namespace Locompro.Data.Repositories;

public interface ISubmissionRepository : ICrudRepository<Submission, SubmissionKey>
{
    /// <summary>
    ///     Gets an entity based on its ID asynchronously.
    /// </summary>
    /// <param name="userId">First part of the primary key</param>
    /// <param name="entryTime">Second part of the primary key</param>
    /// <returns>Entity for the passed ID.</returns>
    Task<Submission> GetByIdAsync(string userId, DateTime entryTime);
    
    /// <summary>
    /// Updates an entity for this repository asynchronously. If the entity does not exist, adds it.
    /// </summary>
    /// <param name="entity">The entity to add or update.</param>
    /// <param name="userId">First part of the primary key</param>
    /// <param name="entryTime">Second part of the primary key</param>
    Task UpdateAsync(string userId, DateTime entryTime, Submission entity);
    
    /// <summary>
    ///     Gets the search results submissions according to the list of search criteria or queries to be used
    /// </summary>
    /// <param name="searchQueries"> search queries, criteria or strategies to be used to find the desired submissions</param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetSearchResults(ISearchQueries searchQueries);

    /// <summary>
    ///     Gets the submissions for a given Item
    /// </summary>
    /// <param name="storeName"></param>
    /// <param name="productName"></param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName);
}