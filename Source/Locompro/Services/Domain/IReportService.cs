using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

/// <summary>
/// Defines the contract for services dealing with reports in the domain.
/// </summary>
public interface IReportService : IDomainService<Report, string>
{
    /// <summary>
    /// Updates the report based on the provided UserReportDto.
    /// </summary>
    /// <param name="userReportDto">The DTO containing the updated report information.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateUserReportAsync(UserReportDto userReportDto);

    /// <summary>
    /// Adds a list of automatic reports to the database.
    /// </summary>
    /// <param name="listOfAutomaticReports"> List of reports to add</param>
    /// <returns>An async operation</returns>
    Task AddManyAutomaticReports(List<AutoReportDto> listOfAutomaticReports);
}