using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

/// <summary>
/// Defines the contract for services dealing with reports in the domain.
/// </summary>
public interface IReportService : IDomainService<Report, string>
{
    /// <summary>
    /// Updates the report based on the provided ReportDto.
    /// </summary>
    /// <param name="reportDto">The DTO containing the updated report information.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(ReportDto reportDto);
}
