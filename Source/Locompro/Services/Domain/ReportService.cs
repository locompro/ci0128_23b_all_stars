using Locompro.Common.Mappers;
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

        Report report = reportFactory.FromDto(reportDto);

        await CrudRepository.AddAsync(report);

        await UnitOfWork.SaveChangesAsync();
    }
}