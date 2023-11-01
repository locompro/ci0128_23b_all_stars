using System.Text.Json;
using System.Text.Json.Serialization;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Account;

/// <summary>
///     Represents the profile page for authenticated users.
/// </summary>
[Authorize]
public class ProfileModel : PageModel
{
    private readonly IDomainService<User, string> _userService;
    private readonly IAuthService _authService;
    private readonly IDomainService<Canton, string> _cantonService;
    
    [BindProperty] public ProfileViewModel UserProfile { get; set; }

    [BindProperty] public PasswordChangeViewModel PasswordChange{ get; set; }

    [BindProperty] public UserDataUpdateViewModel UserDataUpdate { get; set; }
    
    public bool HasErrorsInChangePasswordModal
        => ModelState.ContainsKey("Modal-1") && ModelState["Modal-1"]?.Errors is { Count: > 0 };

    public bool IsPasswordChanged { get; set; } = false;
    
    public bool IsUserDataUpdated { get; set; } = false;


    /// <summary>
    ///     Initializes a new instance of the ProfileModel class.
    /// </summary>
    /// <param name="userService">Service for user related operations.</param>
    /// <param name="authService">Service for authentication related operations.</param>
    /// <param name="cantonService"></param>
    public ProfileModel(IDomainService<User, string> userService, IAuthService authService, IDomainService<Canton, string> cantonService)
    {
        _userService = userService;
        _authService = authService;
        _cantonService = cantonService;
    }
    public async Task<JsonResult> OnGetCantons(string provinceName)
    {
        var cantons = await _cantonService.GetAll();
        if (cantons == null || string.IsNullOrWhiteSpace(provinceName))
        {
            return new JsonResult(new List<CantonDto>());
        }

        var cantonNames = cantons
            .Where(c => c.ProvinceName == provinceName)
            .Select(c => new CantonDto { Name = c.Name })
            .ToList();

        return new JsonResult(cantonNames);
    }
    
    /// <summary>
    ///     Handler for the GET request to load the user's profile page.
    /// </summary>
    /// <returns>The user's profile page.</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userService.Get(_authService.GetUserId());
        if (user == null) return NotFound("Unable to load user.");
        SetProfileViewModel(user);
        RecoverTemporaryFlags();
        return Page();
    }

    /// <summary>
    ///     Handler for the POST request to change the user's password.
    /// </summary>
    /// <returns>Redirects to the user profile page.</returns>
    public async Task<IActionResult> OnPostChangePasswordAsync()
    {
        var user = await _userService.Get(_authService.GetUserId());
        if (user == null) return RedirectToRoute("Account/Login");

        if (!await _authService.IsCurrentPasswordCorrect(PasswordChange.CurrentPassword))
        {
            AppendErrorToChangePasswordModal("La contraseña actual no es correcta.");
            SetProfileViewModel(user);
            return Page();
        }

        await _authService.ChangePassword(PasswordChange.CurrentPassword, PasswordChange.NewPassword);

        await _authService.RefreshUserLogin();

        StoreTemporaryFlag("IsPasswordChanged", true);
        return RedirectToPage();
    }
    /// <summary>
    ///     Handler for the POST request to modify the user's data.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostUpdateUserDataAsync()
    {
        var user = await _userService.Get(_authService.GetUserId());

        if (user == null) return RedirectToRoute("Account/Login");

        if (UserDataUpdate.IsEmpty()) return RedirectToPage();

        user.Email = UserDataUpdate.IsEmailEmpty() ? user.Email : UserDataUpdate.Email;
        user.NormalizedEmail = UserDataUpdate.IsEmailEmpty() ? user.NormalizedEmail : UserDataUpdate.Email.ToUpper();
        user.Address = UserDataUpdate.IsAddressEmpty() ? user.Address : UserDataUpdate.GetAddress();

        await _userService.Update(user);
        StoreTemporaryFlag("IsUserDataUpdated", true);
        return RedirectToPage();
    }

    /// <summary>
    ///     Prepare the ProfileViewModel to be displayed on the page.
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
    ///   Stores the status of the user data modification operation in TempData so it can be recovered after a redirect.
    /// </summary>
    /// <param name="key"> the temporary data name</param>
    /// <param name="value"> the value of the boolean flag </param>
    private void StoreTemporaryFlag(string key, bool value)
    {
        TempData[key] = value;
    }
    
    /// <summary>
    ///   Recovers the status of the user data modification operation from TempData.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool RecoverTemporaryFlag(string key)
    {
        var value = TempData[key] as bool?;
        return value ?? false;
    }
    /// <summary>
    ///     Recovers all the status flags of the user data modification operation from TempData.
    /// </summary>
    private void RecoverTemporaryFlags()
    {
        IsPasswordChanged = RecoverTemporaryFlag("IsPasswordChanged");
        IsUserDataUpdated = RecoverTemporaryFlag("IsUserDataUpdated");
    }
   
    /// <summary>
    ///     Appends an error to the Change Password modal.
    /// </summary>
    /// <param name="error"> a error message to append </param>
    private void AppendErrorToChangePasswordModal(string error)
    {
        ModelState.AddModelError("Modal-1", error);
    }
}