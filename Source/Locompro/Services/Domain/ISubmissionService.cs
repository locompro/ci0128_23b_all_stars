using Locompro.Data.Repositories;
using Locompro.Models;
using Locompro.SearchQueryConstruction;

namespace Locompro.Services.Domain;

public interface ISubmissionService : IDomainService<Submission, SubmissionKey>
{
    /// <summary>
    /// Gets the search results submissions according to the list of search criteria or queries to be used
    /// </summary>
    /// <param name="searchQueries"> search queries, criteria or strategies to be used to find the desired submissions</param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries);
    
    /// <summary>
    /// Gets submissions containing a specific product name
    /// </summary>
    /// <param name="productName"></param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetByProductName(string productName);

    /// <summary>
    /// Gets submissions containing a specific product model
    /// </summary>
    /// <remarks> This is just a wrapper for the submission repository </remarks>
    Task<IEnumerable<Submission>> GetByProductModel(string productModel);

    /// <summary>
    /// Calls the submission repository to get all submissions containing a specific brand name
    /// </summary>
    /// <param name="brandName"></param>
    /// <returns> An Enumerable with al the submissions tha meet the criteria</returns>
    Task<IEnumerable<Submission>> GetByBrand(string brandName);

    Task<IEnumerable<Submission>> GetByCantonAndProvince(string canton, string province);
    Task<IEnumerable<Submission>> GetByCanton(string canton, string province);
}