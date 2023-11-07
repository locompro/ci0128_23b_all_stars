﻿using System.Security.Claims;
using Locompro.Common;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Common.Mappers;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Results;
using Locompro.Models.ViewModels;
using Locompro.Services.Auth;
using Locompro.Services.Domain;

namespace Locompro.Services;

/// <summary>
///     Provides services for moderating users, including assigning moderator roles to qualified users.
/// </summary>
public class ModerationService : Service, IModerationService
{
    private readonly string[] _rolesCheckedInAssignment =
        { RoleNames.Moderator, RoleNames.RejectedModeratorRole, RoleNames.PossibleModerator };

    private readonly IUserManagerService _userManagerService;
    private readonly IUserService _userService;
    private readonly ISubmissionService _submissionService;
    private readonly IReportService _reportService; 

    public ModerationService(ILoggerFactory loggerFactory,
        IUserService userService,
        IUserManagerService userManagerService,
        ISubmissionService submissionService,
        IReportService reportService) : base(loggerFactory)
    {
        _userService = userService;
        _userManagerService = userManagerService;
        _submissionService = submissionService;
        _reportService = reportService;
    }

    /// <summary>
    ///     Asynchronously assigns the 'PossibleModerator' role to qualified users.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AssignPossibleModeratorsAsync()
    {
        // Get the list of users who are qualified to be moderators.
        var qualifiedUserIDs = GetQualifiedUserIDs();

        var tasks = qualifiedUserIDs.Select(user => AddPossibleModeratorRoleAsync(user.Id)).ToList();

        await Task.WhenAll(tasks);
    }

    /// <inheritdoc />
    public async Task ReportSubmission(ReportDto reportDto)
    {
        await _reportService.Add(reportDto);
    }

    /// <summary>
    ///     Retrieves a list of user IDs for users who are qualified to be moderators.
    /// </summary>
    /// <returns>A list of qualified user IDs.</returns>
    private List<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        return _userService.GetQualifiedUserIDs();
    }

    /// <summary>
    ///     Asynchronously assigns the 'PossibleModerator' role to a user, if they don't already have the 'Moderator' role.
    /// </summary>
    /// <param name="userID">The ID of the user to potentially assign the role to.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task AddPossibleModeratorRoleAsync(string userID)
    {
        var user = await _userManagerService.FindByIdAsync(userID);
        if (user == null)
        {
            Logger.LogError($"Could not find user with ID '{userID}'.");
            return;
        }

        if (await IsUserInAnyIncompatibleRoleAsync(user, _rolesCheckedInAssignment))
        {
            Logger.LogInformation($"User with ID '{userID}' is already in an incompatible role.");
            return;
        }

        var result =
            await _userManagerService.AddClaimAsync(user, new Claim(ClaimTypes.Role, RoleNames.PossibleModerator));

        if (!result.Succeeded)
            Logger.LogError($"Could not assign 'PossibleModerator' role to user with ID '{userID}'.");
        else
            Logger.LogInformation($"Assigned 'PossibleModerator' role to user with ID '{userID}'.");
    }

    /// <summary>
    ///     Asynchronously determines if the specified user is in any of the given roles that are considered incompatible.
    /// </summary>
    /// <param name="user">The user to check for incompatible roles.</param>
    /// <param name="rolesToCheck">The roles to check against the user's current roles.</param>
    /// <returns>
    ///     The task result contains a boolean value that is true if the user is in any of the roles provided; otherwise,
    ///     false.
    /// </returns>
    private async Task<bool> IsUserInAnyIncompatibleRoleAsync(User user, IEnumerable<string> rolesToCheck)
    {
        var userRoles = await _userManagerService.GetClaimsOfTypesAsync(user, ClaimTypes.Role);
        return rolesToCheck.Any(role => userRoles.Any(userRole => userRole.Value == role));
    }

    /// <inheritdoc />
    public async Task ActOnReport(ModeratorActionOnReportVm moderatorActionOnReportVm)
    {
        SubmissionKey submissionKey = new ()
        {
            UserId = moderatorActionOnReportVm.SubmissionUserId,
            EntryTime = moderatorActionOnReportVm.SubmissionEntryTime
        };
        
        switch (moderatorActionOnReportVm.Action)
        {
            case ModeratorActions.EraseSubmission:
                await _submissionService.DeleteSubmissionAsync(submissionKey);
                break;
            case ModeratorActions.EraseReport:
                await _submissionService.UpdateSubmissionStatusAsync(submissionKey, SubmissionStatus.Moderated);
                break;
            case ModeratorActions.Default:
            default:
                throw new ArgumentOutOfRangeException(nameof(moderatorActionOnReportVm));
        }
    }
}