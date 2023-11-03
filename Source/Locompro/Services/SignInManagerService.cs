using System.Security.Claims;
using System.Threading.Tasks;
using Locompro.Models;
using Locompro.Services.AuthInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Locompro.Services.Domain;

public class SignInManagerService : ISignInManagerService
{
    private readonly SignInManager<User> _signInManager;

    public SignInManagerService(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public Task SignInAsync(User user, bool isPersistent)
    {
        return _signInManager.SignInAsync(user, isPersistent);
    }

    public Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        return _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }

    public Task SignOutAsync()
    {
        return _signInManager.SignOutAsync();
    }

    public bool IsSignedIn(ClaimsPrincipal principal)
    {
        return _signInManager.IsSignedIn(principal);
    }

    public HttpContext Context => _signInManager.Context;

    public Task RefreshSignInAsync(User user)
    {
        return _signInManager.RefreshSignInAsync(user);
    }
}