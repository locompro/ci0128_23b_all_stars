using System.Security.Claims;
using Locompro.Common;
using Locompro.Models;
using Locompro.Services.Auth;
using Locompro.Services.Domain;

namespace Locompro.Services;

/// <summary>
/// Provides services for moderating users, including assigning moderator roles to qualified users.
/// </summary>
public class ModerationService : IModerationService
{
    private readonly IUserService _userService;
    private readonly IUserManagerService _userManagerService;
    private readonly ILogger<ModerationService> _logger;

    public ModerationService(ILoggerFactory loggerFactory, IUserService userService,
        IUserManagerService userManagerService)
    {
        _userService = userService;
        _userManagerService = userManagerService;
        _logger = loggerFactory.CreateLogger<ModerationService>();
    }

    /// <summary>
    /// Asynchronously assigns the 'PossibleModerator' role to qualified users.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AssignPossibleModeratorsAsync()
    {
        // Get the list of users who are qualified to be moderators.
        var qualifiedUserIDs = GetQualifiedUserIDs();
        foreach (var qualifiedUserId in qualifiedUserIDs)
        {
            await AddPossibleModeratorRoleAsync(qualifiedUserId.Id);
        }
    }

    /// <summary>
    /// Retrieves a list of user IDs for users who are qualified to be moderators.
    /// </summary>
    /// <returns>A list of qualified user IDs.</returns>
    private List<GetQualifiedUserIDsResult> GetQualifiedUserIDs()
    {
        return _userService.GetQualifiedUserIDs();
    }

    /// <summary>
    /// Asynchronously assigns the 'PossibleModerator' role to a user, if they don't already have the 'Moderator' role.
    /// </summary>
    /// <param name="userID">The ID of the user to potentially assign the role to.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the user cannot be found or if the role cannot be assigned.</exception>
    private async Task AddPossibleModeratorRoleAsync(string userID)
    {
        var user = await _userManagerService.FindByIdAsync(userID);
        if (user == null)
        {
            _logger.LogError($"Could not find user with ID '{userID}'.");
        }

        if (await _userManagerService.IsInRoleAsync(user, RoleNames.Moderator))
        {
            _logger.LogInformation($"User with ID '{userID}' is already a moderator.");
            return;
        }

        var result =
            await _userManagerService.AddClaimAsync(user, new Claim(ClaimTypes.Role, RoleNames.PossibleModerator));

        if (!result.Succeeded)
        {
            _logger.LogError($"Could not assign 'PossibleModerator' role to user with ID '{userID}'.");
        }
        else
        {
            _logger.LogInformation($"Assigned 'PossibleModerator' role to user with ID '{userID}'.");
        }
    }
}