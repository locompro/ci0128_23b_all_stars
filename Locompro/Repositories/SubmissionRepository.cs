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