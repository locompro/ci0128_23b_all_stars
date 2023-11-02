using Locompro.Common.Search;
using Locompro.Models;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

public struct SubmissionKey
{
    public string CountryName { get; set; }
    public DateTime EntryTime { get; set; }
}

/// <summary>
/// Repository for Submission entities.
/// Key is a tuple of the country name and the Datetime of the submission.
/// </summary>
public class SubmissionRepository : CrudRepository<Submission, SubmissionKey>, ISubmissionRepository
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="loggerFactory"></param>
    public SubmissionRepository(DbContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetSearchResults(SearchQueries searchQueries)
    {
        // initiate the query
        IQueryable<Submission> submissionsResults = Set
            .Include(submission => submission.Product);

        // append the search queries to the query
        submissionsResults =
            searchQueries.SearchQueryFunctions.Aggregate(submissionsResults, (current, query) => current.Where(query));

        // get and return the results
        return await submissionsResults.ToListAsync();
    }
}