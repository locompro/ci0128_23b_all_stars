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
    
    /// <summary>
    /// Checks if a user is in a given role
    /// </summary>
    /// <param name="user"> The user</param>
    /// <param name="role"> The name of the role to check</param>
    /// <returns>A task representing the role search</returns>
    public Task<bool> IsInRoleAsync(User user, string role)
    {
        return _userManager.GetClaimsAsync(user).ContinueWith(task =>
        {
            var claims = task.Result;
            return claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role);
        });
    }
    
    /// <summary>
    /// Asynchronously finds a user based on the user ID.
    /// </summary>
    /// <param name="userId">The user's ID to search for.</param>
    /// <returns>
    /// The task result contains
    /// the user corresponding to the specified ID if found; otherwise, null.
    /// </returns>
    public Task<User> FindByIdAsync(string userId)
    {
        return _userManager.FindByIdAsync(userId);
    }
    
    /// <summary>
    /// Asynchronously adds a claim to a user.
    /// </summary>
    /// <param name="user">The user to add the claim to.</param>
    /// <param name="claim">The claim to add.</param>
    /// <returns>
    /// A task that represents the asynchronous operation of adding a claim to the user.
    /// The task result contains an <see cref="IdentityResult"/> indicating the success
    /// or failure of the operation.
    /// </returns>
    public Task<IdentityResult> AddClaimAsync(User user, Claim claim)
    {
        return _userManager.AddClaimAsync(user, claim);
    }
}