using System.Security.Claims;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services.Auth;

/// <summary>
///     Provides an interface for user sign-in operations within the application.
/// </summary>
public interface ISignInManagerService
{
    /// <summary>
    ///     Asynchronously signs in the specified user.
    /// </summary>
    /// <param name="user">The user to sign in.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous sign in operation.</returns>
    Task SignInAsync(User user, bool isPersistent);

    /// <summary>
    ///     Attempts to sign in the specified username and password combination as a user.
    /// </summary>
    /// <param name="userName">The user name to be signed in.</param>
    /// <param name="password">The password to be used.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked upon failure.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation with a result of <see cref="SignInResult" />.</returns>
    Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);

    /// <summary>
    ///     Asynchronously signs out the current user.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous sign out operation.</returns>
    Task SignOutAsync();

    /// <summary>
    ///     Checks if the given claims principal is signed in.
    /// </summary>
    /// <param name="principal">The claims principal to check.</param>
    /// <returns>A <see cref="bool" /> indicating whether the claims principal is signed in.</returns>
    bool IsSignedIn(ClaimsPrincipal principal);

    /// <summary>
    ///     Gets the current HTTP context.
    /// </summary>
    HttpContext Context { get; }

    /// <summary>
    ///     Asynchronously refreshes the sign-in for the specified user.
    /// </summary>
    /// <param name="user">The user whose sign-in information should be refreshed.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task RefreshSignInAsync(User user);
}