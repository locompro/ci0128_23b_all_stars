using System.Security.Claims;
using System.Threading.Tasks;
using Locompro.Models;
using Locompro.Services.AuthInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<User> _userManager;

        public UserManagerService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            return _userManager.GetUserAsync(principal);
        }

        public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public bool SupportsUserEmail => _userManager.SupportsUserEmail;

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }
    }
}