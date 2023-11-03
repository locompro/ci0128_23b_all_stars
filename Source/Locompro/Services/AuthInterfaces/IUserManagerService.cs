using System.Security.Claims;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services.AuthInterfaces;

public interface IUserManagerService
{
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<User> GetUserAsync(ClaimsPrincipal principal);
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    bool SupportsUserEmail { get; }
    Task<bool> CheckPasswordAsync(User user, string password);
}