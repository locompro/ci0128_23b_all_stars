using Locompro.Data;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Factories;

namespace Locompro.Services.Domain;

public class ReportService : DomainService<Report, string>, IReportService
{
    public ReportService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
    {
    }

    public async Task Add(ReportDto reportDto)
    {
        var reportFactory = new ReportFactory();

        var report = reportFactory.FromDto(reportDto);

        try
        {
            await CrudRepository.AddAsync(report);

            await UnitOfWork.SaveChangesAsync();
            Logger.LogInformation("Successfully added report {}", reportDto);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to add report {}", reportDto);
            throw;
        }
    }
}