using Locompro.Common.Search;
using Locompro.Models;

namespace Locompro.Data.Repositories;

public interface ISubmissionRepository : ICrudRepository<Submission, SubmissionKey>
{
    /// <summary>
    /// Gets the search results submissions according to the list of search criteria or queries to be used
    /// </summary>
    /// <param name="searchQueries"> search queries, criteria or strategies to be used to find the desired submissions</param>
    /// <returns></returns>
    Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries);
}