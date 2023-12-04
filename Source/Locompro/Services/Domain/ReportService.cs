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
    public async Task UpdateUserReportAsync(UserReportDto userReportDto)
    {
        var reportFactory = new ReportFactory();

        var submissionUserId = userReportDto.SubmissionUserId;
        var submissionEntryTime = userReportDto.SubmissionEntryTime;
        var userId = userReportDto.UserId;
        var report = reportFactory.FromDto(userReportDto);

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

    /// <summary>
    /// Adds multiple automatic reports to the system.
    /// </summary>
    /// <param name="listOfAutomaticReports">The list of automatic reports to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddManyAutomaticReports(List<AutoReportDto> listOfAutomaticReports)
    {
        var reportFactory = new AutoReportFactory();
        var reports = listOfAutomaticReports.Select(reportFactory.FromDto).ToList();
        try
        {
            await _reportRepository.AddOrUpdateManyAutomaticReports(reports);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to add automatic reports");
            throw;
        }
    }
}