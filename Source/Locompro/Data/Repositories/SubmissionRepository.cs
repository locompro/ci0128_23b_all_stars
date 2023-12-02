using Locompro.Common.Search;
using Locompro.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

public struct SubmissionKey
{
    public string UserId { get; set; }
    public DateTime EntryTime { get; set; }
}

/// <summary>
///     Repository for Submission entities.
///     Key is a tuple of the country name and the Datetime of the submission.
/// </summary>
public class SubmissionRepository : CrudRepository<Submission, SubmissionKey>, ISubmissionRepository
{
    /// <summary>
    ///     Constructor
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
        Submission submission = await Set.FirstOrDefaultAsync(submission =>
            submission.UserId == id.UserId &&
            submission.EntryTime == id.EntryTime);

        if (submission == null)
        {
            throw new InvalidOperationException("Error loading submission! No submission for user:" + id.UserId + " and entry time: " +
                                                id.EntryTime + " was found.");
        }
        
        return submission;
    }

    /// <inheritdoc />
    public override async Task DeleteAsync(SubmissionKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null) Set.Remove(entity);
    }

    public async Task<Submission> GetByIdAsync(string userId, DateTime entryTime)
    {
        return await Set.FindAsync(userId, entryTime);
    }

    public async Task UpdateAsync(string userId, DateTime entryTime, Submission entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        
        var existingEntity = await GetByIdAsync(userId, entryTime);
        if (existingEntity != null)
        {
            Context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();
        }
        else
        {
            await AddAsync(entity);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetSearchResults(ISearchQueries searchQueries)
    {
        // initiate the query
        IQueryable<Submission> submissionsResults = Set
            .Include(submission => submission.Product);

        // append the search queries to the query
        submissionsResults = searchQueries.ApplySearch(submissionsResults) as IQueryable<Submission> ;

        if (submissionsResults == null)
        {
            return await new Task<IEnumerable<Submission>>(() => new List<Submission>());
        }
        
        // get and return the results
        return await submissionsResults.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Submission>> GetItemSubmissions(string storeName, string productName)
    {
        var submissionsResults = Set
            .Include(submission => submission.Product)
            .Where(submission => submission.Product.Name == productName && submission.Store.Name == storeName);

        return await submissionsResults.ToListAsync();
    }
}