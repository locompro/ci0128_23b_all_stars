#nullable disable

using Locompro.Areas.Identity.ViewModels;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;

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

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<User>)_userStore;
        }

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
    }
}