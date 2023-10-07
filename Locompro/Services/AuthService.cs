#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Locompro.Areas.Identity.ViewModels;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Locompro.Services
{
    public class AuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterViewModel> _logger;

        public AuthService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            ILogger<RegisterViewModel> logger,
            IUserEmailStore<User> emailStore = null)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _logger = logger;
            _emailStore = emailStore ?? GetEmailStore();
        }
        /// <summary>
        /// Creates a new instance of the <see cref="User"/> class.
        /// </summary>
       
        /// <returns>A new instance of the <see cref="User"/> class.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the <see cref="User"/> class cannot be instantiated. This could be due to the class being abstract, 
        /// lacking a parameterless constructor, or other reflection-related issues. When this exception is thrown, 
        /// consider overriding the registration page located at /Areas/Identity/Pages/Account/Register.cshtml.
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
        /// Register a user with the given data using ASP.NET Core Identity.
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
                _logger.LogInformation("User created a new account with password.");
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        /// <summary>
        /// 
        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<User>)_userStore;
        }
        /// <summary>
        /// Attempts to sign in a user using the provided username and password.
        /// </summary>
        /// <param name="inputData">A view model containing the user's login details.</param>
        /// <returns>The result of the sign-in attempt.</returns>
        /// <remarks>
        /// If the login is successful, a log entry will be created stating "User logged in."
        /// The method will not lock out the user even after multiple failed login attempts.
        /// </remarks>
        public async Task<SignInResult> Login(LoginViewModel inputData)
        {
            var result = await _signInManager.PasswordSignInAsync(inputData.UserName, inputData.Password,
                inputData.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
            }

            return result;
        }
        /// <summary>
        /// Signs out the current logged-in user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// Once the user is logged out, a log entry will be created stating "User logged out."
        /// </remarks>
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
        }
        /// <summary>
        /// Checks if a user is currently logged in.
        /// </summary>
        /// <returns>True if a user is logged in, otherwise false.</returns>
        public bool IsLoggedIn()
        {
         var result =  _signInManager.IsSignedIn(_signInManager.Context.User);
         return result;
        }
    }
}

