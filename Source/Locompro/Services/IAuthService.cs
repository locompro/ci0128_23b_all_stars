using Locompro.Areas.Identity.ViewModels;
using Locompro.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services;

public interface IAuthService
{
    /// <summary>
    /// Register a user with the given data using ASP.NET Core Identity.
    /// </summary>
    /// <param name="inputData">Data entered by the user in the view</param>
    /// <returns>The result of the registration attempt.</returns>
    Task<IdentityResult> Register(RegisterViewModel inputData);

    /// <summary>
    /// Attempts to sign in a user using the provided username and password.
    /// </summary>
    /// <param name="inputData">A view model containing the user's login details.</param>
    /// <returns>The result of the sign-in attempt.</returns>
    /// <remarks>
    /// If the login is successful, a log entry will be created stating "User logged in."
    /// The method will not lock out the user even after multiple failed login attempts.
    /// </remarks>
    Task<SignInResult> Login(LoginViewModel inputData);

    /// <summary>
    /// Signs out the current logged-in user.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// Once the user is logged out, a log entry will be created stating "User logged out."
    /// </remarks>
    Task Logout();

    /// <summary>
    /// Checks if a user is currently logged in.
    /// </summary>
    /// <returns>True if a user is logged in, otherwise false.</returns>
    bool IsLoggedIn();

    /// <summary>
    /// Gets the ID of the currently logged-in user.
    /// </summary>
    /// <returns> returns a string with the user id</returns>
    string GetUserId();

    /// <summary>
    ///  checks if a given password is correct for a given user
    /// </summary>
    /// <param name="user"> user to check the password</param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<bool> IsCurrentPasswordCorrect(string password);

    /// <summary>
    ///  changes the password of a given user
    /// </summary>
    /// <param name="currentPassword"> current user password</param>
    /// <param name="newPassword"> new password </param>
    /// <returns> results of the operation </returns>
    Task<IdentityResult> ChangePassword(string currentPassword, string newPassword);

    Task RefreshUserLogin();
}