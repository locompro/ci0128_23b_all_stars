using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Services;

/// <summary>
///     Defines a contract for a service that handles the assignment of moderators within the application.
/// </summary>
public interface IModerationService
{
    /// <summary>
    ///     Asynchronously assigns possible moderators to qualified users.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task AssignPossibleModeratorsAsync();

    /// <summary>
    /// Enforces user's decision on whether they will assume the moderator role 
    /// </summary>
    /// <param name="userId">The ID of the user to potentially assign the role to.</param>
    /// <param name="didAcceptRole">Whether the user accepted the moderator role.</param>
    Task DecideOnModeratorRole(string userId, bool didAcceptRole);

    /// <summary>
    /// Acts on a report
    /// </summary>
    /// <param name="moderatorActionDto"></param>
    /// <returns></returns>
    Task ActOnReport(ModeratorActionDto moderatorActionDto);

    /// <summary>
    ///     Adds a report for a given submission
    /// </summary>
    /// <param name="reportDto">Report to be added</param>
    Task ReportSubmission(ReportDto reportDto);

    /// <summary>
    /// Returns whether the user has the Possible Moderator role
    /// </summary>
    /// <param name="userId">ID of user to check for role</param>
    /// <returns>whether the user has the Possible Moderator role</returns>
    Task<bool> IsUserPossibleModerator(string userId);
}