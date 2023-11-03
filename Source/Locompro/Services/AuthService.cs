using System.Security.Claims;
using Locompro.Areas.Identity.ViewModels;
using Locompro.Data;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services.AuthInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services;

public class AuthService : Service, IAuthService
{
    private readonly ISignInManagerService _signInManager;
    private readonly IUserManagerService _userManager;
    private readonly IUserStore<User> _userStore;
    private readonly IUserEmailStore<User> _emailStore;

    public AuthService(
        IUnitOfWork unitOfWork,
        ILoggerFactory loggerFactory,
        ISignInManagerService signInManager,
        IUserManagerService userManager,
        IUserStore<User> userStore,
        IUserEmailStore<User> emailStore = null) : base(unitOfWork, loggerFactory)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = emailStore ?? GetEmailStore();
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="User" /> class.
    /// </summary>
    /// <returns>A new instance of the <see cref="User" /> class.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the <see cref="User" /> class cannot be instantiated. This could be due to the class being abstract,
    ///     lacking a parameterless constructor, or other reflection-related issues. When this exception is thrown,
    ///     consider overriding the registration page located at /Areas/Identity/Pages/Account/Register.cshtml.
    /// </exception>
    private static User CreateUser()
    {
        try
        {
            return Activator.CreateInstance<User>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                                                $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    /// <summary>
    ///     Register a user with the given data using ASP.NET Core Identity.
    /// </summary>
    /// <param name="inputData">Data entered by the user in the view</param>
    /// <returns>The result of the registration attempt.</returns>
    public async Task<IdentityResult> Register(RegisterViewModel inputData)
    {
        var user = CreateUser();

        await _userStore.SetUserNameAsync(user, inputData.UserName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, inputData.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, inputData.Password);

        if (result.Succeeded)
        {
            Logger.LogInformation("User created a new account with password.");
            await _signInManager.SignInAsync(user, false);
        }

        return result;
    }

    /// <summary>
    ///     Retrieves the user email store associated with the current user manager instance.
    /// </summary>
    /// <returns>
    ///     An instance of the user email store implementing the <see cref="IUserEmailStore{TUser}" /> interface.
    /// </returns>
    /// <remarks>
    ///     This method is used to obtain an instance of the user email store, which allows managing user email addresses
    ///     and related operations such as email confirmation and password reset.
    /// </remarks>
    private IUserEmailStore<User> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
            throw new NotSupportedException("The default UI requires a user store with email support.");

        return (IUserEmailStore<User>)_userStore;
    }

    /// <summary>
    ///     Attempts to sign in a user using the provided username and password.
    /// </summary>
    /// <param name="inputData">A view model containing the user's login details.</param>
    /// <returns>The result of the sign-in attempt.</returns>
    /// <remarks>
    ///     If the login is successful, a log entry will be created stating "User logged in."
    ///     The method will not lock out the user even after multiple failed login attempts.
    /// </remarks>
    public async Task<SignInResult> Login(LoginViewModel inputData)
    {
        var result = await _signInManager.PasswordSignInAsync(inputData.UserName, inputData.Password,
            inputData.RememberMe, false);
        if (result.Succeeded) Logger.LogInformation("User logged in.");

        return result;
    }

    /// <summary>
    ///     Signs out the current logged-in user.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     Once the user is logged out, a log entry will be created stating "User logged out."
    /// </remarks>
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
        Logger.LogInformation("User {id} logged out.", GetUserId());
    }

    /// <summary>
    ///     Checks if a user is currently logged in.
    /// </summary>
    /// <returns>True if a user is logged in, otherwise false.</returns>
    public bool IsLoggedIn()
    {
        return _signInManager.IsSignedIn(_signInManager.Context.User);
    }

    /// <summary>
    ///     Gets the ID of the currently logged-in user.
    /// </summary>
    /// <returns> returns a string with the user id</returns>
    public string GetUserId()
    {
        return _signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    /// <summary>
    ///     checks if a given password is correct for a given user
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> IsCurrentPasswordCorrect(string password)
    {
        var user = await _userManager.GetUserAsync(_signInManager.Context.User);
        return await _userManager.CheckPasswordAsync(user, password);
    }

    /// <summary>
    ///     Changes the password of a given user
    /// </summary>
    /// <param name="currentPassword"> current user password</param>
    /// <param name="newPassword"> new password </param>
    /// <returns> results of the operation </returns>
    public async Task<IdentityResult> ChangePassword(string currentPassword, string newPassword)
    {
        var user = await _userManager.GetUserAsync(_signInManager.Context.User);
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    /// <summary>
    ///   Refreshes the user login.
    /// </summary>
    public async Task RefreshUserLogin()
    {
        var user = await _userManager.GetUserAsync(_signInManager.Context.User);
        await _signInManager.RefreshSignInAsync(user);
    }
}