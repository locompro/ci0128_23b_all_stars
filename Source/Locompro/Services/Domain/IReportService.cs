using Locompro.Models.Dtos;
using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

/// <summary>
/// Defines the contract for services dealing with reports in the domain.
/// </summary>
public interface IReportService : IDomainService<Report, string>
{
    /// <summary>
    /// Gets all reports by a given user Id
    /// </summary>
    /// <param name="userId">User Id for which to get all reports</param>
    Task<IEnumerable<Report>> GetByUserId(string userId);
    
    
    /// <summary>
    /// Updates an existing report or adds a new one based on the provided <see cref="ReportDto"/>.
    /// </summary>
    /// <param name="userReportDto">The data transfer object containing the report data to update or add.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    /// <exception cref="Exception">Thrown when the update fails.</exception>
    Task UpdateUserReportAsync(UserReportDto userReportDto);

    /// <summary>
    /// Adds a list of automatic reports to the database.
    /// </summary>
    /// <param name="listOfAutomaticReports"> List of reports to add</param>
    /// <returns>An async operation</returns>
    Task AddManyAutomaticReports(List<AutoReportDto> listOfAutomaticReports);
}