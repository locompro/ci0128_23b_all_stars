using System.Linq.Dynamic.Core;
using Locompro.Common;
using Locompro.Common.Search;
using Locompro.Models;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

public struct SubmissionKey
{
    public string UserId { get; set; }
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
    public override async Task<Submission> GetByIdAsync(SubmissionKey id)
    {
        return await Set.FindAsync(id.UserId, id.EntryTime);    
    }
    
    /// <inheritdoc />
    public override async Task DeleteAsync(SubmissionKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            Set.Remove(entity);
        }
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

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName)
    {
        IQueryable<Submission> submissionsResults = Set
            .Include(submission => submission.Product)
            .Where(submission => submission.Product.Name == productName && submission.Store.Name == storeName);
        
        return await submissionsResults.ToListAsync();
    }

    /// <inheritdoc />
    public async Task UpdateSubmissionRating(string userId, DateTime entryTime, int newRating)
    {
        Submission submissionToUpdate = await Set.SingleOrDefaultAsync(
            submission => submission.UserId== userId &&
                          submission.EntryTime == entryTime);
        
        submissionToUpdate.Rating = newRating;
        
        Context.Update(submissionToUpdate);
    }

    
    
}