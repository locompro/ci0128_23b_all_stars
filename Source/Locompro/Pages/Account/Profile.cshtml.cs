using System.Security.Claims;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Account;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly UserService _userService;
    private readonly AuthService _authService;


    public ProfileModel(UserService userService, AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    public ProfileViewModel UserProfile { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userService.Get(_authService.GetUserId());
        if (user == null)
        {
            return NotFound("Unable to load user.");
        }

        UserProfile = new ProfileViewModel
        {
            Username = user.UserName,
            Name = user.Name,
            Address = user.Address,
            Rating = user.Rating,
            Contributions = user.Submissions.Count,
            Email = user.Email
        };

        return Page();
    }
}