﻿using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

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
    ///     Adds a report for a given submission
    /// </summary>
    /// <param name="reportDto">Report to be added</param>
    Task ReportSubmission(ReportDto reportDto);
}