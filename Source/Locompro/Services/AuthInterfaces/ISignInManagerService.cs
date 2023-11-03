using System.Security.Claims;
using Locompro.Models;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services.AuthInterfaces;

public interface ISignInManagerService
{
    Task SignInAsync(User user, bool isPersistent);
    Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
    Task SignOutAsync();
    bool IsSignedIn(ClaimsPrincipal principal);
    HttpContext Context { get; }
    Task RefreshSignInAsync(User user);
}