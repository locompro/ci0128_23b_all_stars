using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

public interface IReportService : IDomainService<Report, string>
{
    Task UpdateAsync(ReportDto reportDto);
}