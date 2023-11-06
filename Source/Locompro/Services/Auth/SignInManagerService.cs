using System.Security.Claims;
using Locompro.Models;
using Locompro.Services.Auth;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Provides the methods necessary for user sign-in management.
/// </summary>
public class SignInManagerService : ISignInManagerService
{
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignInManagerService"/> class.
    /// </summary>
    /// <param name="signInManager">The SignInManager instance.</param>
    public SignInManagerService(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    /// <summary>
    /// Signs in the specified user.
    /// </summary>
    /// <param name="user">The user to sign in.</param>
    /// <param name="isPersistent">True to create a persistent cookie, otherwise false.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous sign in operation.</returns>
    public Task SignInAsync(User user, bool isPersistent)
    {
        return _signInManager.SignInAsync(user, isPersistent);
    }

    /// <summary>
    /// Attempts to sign in the specified user using a user name and password.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="password">The password.</param>
    /// <param name="isPersistent">True to create a persistent cookie, otherwise false.</param>
    /// <param name="lockoutOnFailure">True to lockout the user upon failed login attempt, otherwise false.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous sign in operation.</returns>
    public Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        return _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }

    /// <summary>
    /// Signs out the current user.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous sign out operation.</returns>
    public Task SignOutAsync()
    {
        return _signInManager.SignOutAsync();
    }

    /// <summary>
    /// Determines whether the specified user is signed in.
    /// </summary>
    /// <param name="principal">The claims principal.</param>
    /// <returns>True if the user is signed in, otherwise false.</returns>
    public bool IsSignedIn(ClaimsPrincipal principal)
    {
        return _signInManager.IsSignedIn(principal);
    }

    /// <summary>
    /// Gets the <see cref="HttpContext"/> for the current request.
    /// </summary>
    public HttpContext Context => _signInManager.Context;

    /// <summary>
    /// Refreshes the sign-in for the specified user.
    /// </summary>
    /// <param name="user">The user whose sign-in should be refreshed.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous sign in refresh operation.</returns>
    public Task RefreshSignInAsync(User user)
    {
        return _signInManager.RefreshSignInAsync(user);
    }
}