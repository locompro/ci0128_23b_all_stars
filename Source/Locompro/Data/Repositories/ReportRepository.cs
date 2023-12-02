using Locompro.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data.Repositories;

/// <inheritdoc />
public class ReportRepository : CrudRepository<Report, string>, IReportRepository
{
    public ReportRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
    }

    /// <inheritdoc />
    public async Task<Report> GetByIdAsync(string submissionUserId, DateTime submissionEntryTime, string userId)
    {
        return await Set.FindAsync(submissionUserId, submissionEntryTime, userId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Report>> GetByUserIdAsync(string userId)
    {
        return await Set.Where(e => e.UserId == userId).ToListAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(string submissionUserId, DateTime submissionEntryTime, string userId, Report entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var existingEntity = await GetByIdAsync(submissionUserId, submissionEntryTime, userId);
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
    public async Task AddOrUpdateManyAutomaticReports(List<AutoReport> autoReports)
    {
        if (autoReports == null) throw new ArgumentNullException(nameof(autoReports));

        foreach (var autoReport in autoReports)
        {
            var existingEntity = await GetByIdAsync(autoReport.SubmissionUserId, autoReport.SubmissionEntryTime,
                autoReport.UserId);
            if (existingEntity != null)
            {
                Context.Entry(existingEntity).CurrentValues.SetValues(autoReport);
            }
            else
            {
                await AddAsync(autoReport);
            }
        }
    }
}