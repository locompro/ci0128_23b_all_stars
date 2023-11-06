using System.Security.Claims;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services.Auth;

/// <summary>
///     Provides methods to manage users in the application.
/// </summary>
public class UserManagerService : IUserManagerService
{
    private readonly UserManager<User> _userManager;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserManagerService" /> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    public UserManagerService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    ///     Creates a new user with the specified password.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="password">The password.</param>
    /// <returns>A task representing the result of the user creation.</returns>
    public Task<IdentityResult> CreateAsync(User user, string password)
    {
        return _userManager.CreateAsync(user, password);
    }

    /// <summary>
    ///     Gets the user corresponding to the specified claims principal.
    /// </summary>
    /// <param name="principal">The claims principal.</param>
    /// <returns>A task representing the result of the user retrieval.</returns>
    public Task<User> GetUserAsync(ClaimsPrincipal principal)
    {
        return _userManager.GetUserAsync(principal);
    }

    /// <summary>
    ///     Changes the password for the specified user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="currentPassword">The current password.</param>
    /// <param name="newPassword">The new password.</param>
    /// <returns>A task representing the result of the password change.</returns>
    public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        return _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    /// <summary>
    ///     Gets a value indicating whether the user manager supports user email.
    /// </summary>
    public bool SupportsUserEmail => _userManager.SupportsUserEmail;

    /// <summary>
    ///     Checks the password for the specified user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="password">The password.</param>
    /// <returns>A task representing the result of the password check.</returns>
    public Task<bool> CheckPasswordAsync(User user, string password)
    {
        return _userManager.CheckPasswordAsync(user, password);
    }
}