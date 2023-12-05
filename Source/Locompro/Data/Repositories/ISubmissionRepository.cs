using Locompro.Common.Search;
using Locompro.Models.Entities;

namespace Locompro.Data.Repositories;

public interface ISubmissionRepository : ICrudRepository<Submission, SubmissionKey>
{
    /// <summary>
    ///     Gets all submissions matching a given user Id
    /// </summary>
    /// <param name="userId">User Id for which to get all submissions</param>
    /// <returns>Submissions create by a given user</returns>
    Task<IEnumerable<Submission>> GetByUserIdAsync(string userId);
    
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
    ///     Gets the submissions for a given Item
    /// </summary>
    /// <param name="storeName"></param>
    /// <param name="productName"></param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName);

    /// <summary>
    /// Gets a summary of product availability at each store
    /// </summary>
    /// <param name="productIds">Ids of products to check for availability</param>
    /// <returns>Summary of product availability at each store</returns>
    Task<IEnumerable<ProductSummaryStore>> GetProductSummaryByStore(List<int> productIds);
}