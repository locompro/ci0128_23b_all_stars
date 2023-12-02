using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Factories;

namespace Locompro.Services.Domain;

/// <summary>
/// Provides service operations for managing reports within the domain.
/// </summary>
public class ReportService : DomainService<Report, string>, IReportService
{
    private readonly IReportRepository _reportRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ReportService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for database transactions.</param>
    /// <param name="loggerFactory">The factory to create loggers from.</param>
    public ReportService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
        _reportRepository = UnitOfWork.GetSpecialRepository<IReportRepository>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Report>> GetByUserId(string userId)
    {
        return await _reportRepository.GetByUserIdAsync(userId);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(ReportDto reportDto)
    {
        var reportFactory = new ReportFactory();

        var submissionUserId = reportDto.SubmissionUserId;
        var submissionEntryTime = reportDto.SubmissionEntryTime;
        var userId = reportDto.UserId;
        var report = reportFactory.FromDto(reportDto);
        
        try
        {
            await _reportRepository.UpdateAsync(submissionUserId, submissionEntryTime, userId, report);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to update or add entity");
            throw;
        }
    }
}
