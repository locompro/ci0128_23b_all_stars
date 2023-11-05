using System.Security.Claims;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services.Auth;

/// <summary>
///     Provides an interface for managing user data within the application.
/// </summary>
public interface IUserManagerService
{
    /// <summary>
    ///     Asynchronously creates a new user with the specified password.
    /// </summary>
    /// <param name="user">The user to be created.</param>
    /// <param name="password">The password for the user.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation with a result of <see cref="IdentityResult" />.</returns>
    Task<IdentityResult> CreateAsync(User user, string password);

    /// <summary>
    ///     Asynchronously retrieves a user based on the provided claims principal.
    /// </summary>
    /// <param name="principal">The claims principal which identifies the user.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation with a result of <see cref="User" />.</returns>
    Task<User> GetUserAsync(ClaimsPrincipal principal);

    /// <summary>
    ///     Asynchronously changes the password for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to change the password.</param>
    /// <param name="currentPassword">The current password of the user.</param>
    /// <param name="newPassword">The new password for the user.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation with a result of <see cref="IdentityResult" />.</returns>
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

    /// <summary>
    ///     Gets a value indicating whether the user manager supports user email.
    /// </summary>
    bool SupportsUserEmail { get; }

    /// <summary>
    ///     Asynchronously checks the password for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to check the password.</param>
    /// <param name="password">The password to check.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation with a result of <see cref="bool" /> indicating
    ///     whether the password is correct.
    /// </returns>
    Task<bool> CheckPasswordAsync(User user, string password);
}