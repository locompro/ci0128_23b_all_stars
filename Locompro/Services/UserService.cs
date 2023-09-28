#nullable disable

using Locompro.Areas.Identity.ViewModels;
using Locompro.Repositories;
using Microsoft.AspNetCore.Identity;


namespace Locompro.Services
{
    public class UserService : AbstractService<User, string, UserRepository>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterViewModel> _logger;

        public UserService(UnitOfWork unitOfWork,
            UserRepository userRepository,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            ILogger<RegisterViewModel> logger) : base(unitOfWork, userRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
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


    }
}
