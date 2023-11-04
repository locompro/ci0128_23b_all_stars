using Locompro.Common.Search;
using Locompro.Data.Repositories;
using Locompro.Models;

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
    /// Gets the submissions for a given Item 
    /// </summary>
    /// <param name="StoreName"></param>
    /// <param name="ProductName"></param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName);
}