using Locompro.Models;
using Locompro.Data;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Repositories;

public struct SubmissionKey
{
    public string CountryName { get; set; }
    public DateTime EntryTime { get; set; }
}

/// <summary>
/// Repository for Submission entities.
/// Key is a tuple of the country name and the Datetime of the submission.
/// </summary>
public class SubmissionRepository : AbstractRepository<Submission, SubmissionKey>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="loggerFactory"></param>
    public SubmissionRepository(LocomproContext context, ILoggerFactory loggerFactory) :
        base(context, loggerFactory)
    {
    }
    
    /// <summary>
    /// Get all the submissions for a product
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Submission>> GetSubmissionsForProduct (int productId)
    {
        // get all submissions for the product
        IQueryable<Submission> submissionsQuery = this.DbSet.Where(s => s.ProductId == productId);
        
        return await submissionsQuery.ToListAsync();
    }

    public async Task<Submission> GetBestSubmission(int productId)
    {
        // get all submissions for the given product
        IQueryable<Submission> submissionsQuery = this.DbSet.Where(s => s.ProductId == productId);
        
        // get the logic for the best submission
        DateTime mostRecentDate = submissionsQuery.Max(s => s.EntryTime);

        // use logic to get that submission
        Submission bestSubmission = submissionsQuery.FirstOrDefault(s=>s.EntryTime == mostRecentDate);
        
        return await Task.FromResult(bestSubmission);
    }

    public async Task<IEnumerable<Submission>> GetSubmissionsByCantonAsync(string cantonName, string provinceName)
    {
        var submissions = await DbSet
            .Include(s => s.Store)
            .ThenInclude(st => st.Canton)
            .Where(s => s.Store.Canton.Name == cantonName && s.Store.Canton.Province.Name == provinceName)
            .ToListAsync();
    
        return submissions;
    }

}