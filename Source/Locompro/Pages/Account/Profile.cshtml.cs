using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Account;

/// <summary>
/// Represents the profile page for authenticated users.
/// </summary>
[Authorize]
public class ProfileModel : PageModel
{
    private readonly IDomainService<User, string> _userService;
    private readonly IAuthService _authService;
    
    [BindProperty]
    public ProfileViewModel UserProfile { get; set; }
    
    [BindProperty]
    public PasswordChageViewModel PasswordChange { get; set; }
    
    public bool HasErrorsInChangePasswordModal 
        => ModelState.ContainsKey("Modal-1") && ModelState["Modal-1"]?.Errors is { Count: > 0 };
    
    public bool IsPasswordChanged { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the ProfileModel class.
    /// </summary>
    /// <param name="userService">Service for user related operations.</param>
    /// <param name="authService">Service for authentication related operations.</param>
    public ProfileModel(IDomainService<User, string> userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }
    
    /// <summary>
    /// Handler for the GET request to load the user's profile page.
    /// </summary>
    /// <returns>The user's profile page.</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        
        var user = await _userService.Get(_authService.GetUserId());
        if (user == null)
        {
            return NotFound("Unable to load user.");
        }

        SetProfileViewModel(user);
        RecoverPasswordChangeStatus();
        return Page();
    }
    
    /// <summary>
    /// Handler for the POST request to change the user's password.
    /// </summary>
    /// <returns>Redirects to the user profile page.</returns>
    public async Task<IActionResult> OnPostChangePasswordAsync()
    {
        var user = await _userService.Get(_authService.GetUserId());
        if (user == null)
        {
            return RedirectToRoute("Account/Login");
        }
        
        if (!await _authService.IsCurrentPasswordCorrect(PasswordChange.CurrentPassword))
        {
            AppendErrorToChangePasswordModal("The current password is incorrect.");
            SetProfileViewModel(user);
            return Page();
        }
        
        await _authService.ChangePassword(PasswordChange.CurrentPassword, PasswordChange.NewPassword);
        
        await _authService.RefreshUserLogin();
        
        StorePasswordChangeStatus(true);
        return RedirectToPage();
    }
    /// <summary>
    /// Prepare the ProfileViewModel to be displayed on the page.
    /// </summary>
    /// <param name="user">The user whose information will be displayed.</param>
    private void SetProfileViewModel(User user)
    {
        UserProfile = new ProfileViewModel
        {
            Username = user.UserName,
            Name = user.Name,
            Address = user.Address,
            Rating = user.Rating,
            Contributions = user.Submissions.Count,
            Email = user.Email
        };
    }
    
    /// <summary>
    ///  Stores the status of the password change operation in TempData so it can be recovered after a redirect.
    /// </summary>
    /// <param name="status"></param>
    private void StorePasswordChangeStatus(bool status)
    {
        TempData["IsPasswordChanged"] = status;
    }
    /// <summary>
    ///  recovers the status of the password change operation from TempData after a redirect.
    /// </summary>
    private void RecoverPasswordChangeStatus()
    {
        IsPasswordChanged = TempData["IsPasswordChanged"] as bool? ?? false;
    }
    /// <summary>
    /// Appends an error to the Change Password modal.
    /// </summary>
    /// <param name="error"> a error message to append </param>
    private void AppendErrorToChangePasswordModal(string error)
    {
        ModelState.AddModelError("Modal-1", error);
    }
}