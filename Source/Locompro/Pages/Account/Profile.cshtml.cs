using Locompro.Common;
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

    [BindProperty] public PasswordChangeViewModel PasswordChange { get; set; }

    [BindProperty] public UserDataUpdateViewModel UserDataUpdate { get; set; }

    public IErrorStore ChangePasswordModalErrors { get; set; }

    public IErrorStore UpdateUserDataModalErrors { get; set; }

    public bool IsPasswordChanged { get; set; }

    public bool IsUserDataUpdated { get; set; }


    /// <summary>
    ///     Initializes a new instance of the ProfileModel class.
    /// </summary>
    /// <param name="userService">Service for user related operations.</param>
    /// <param name="authService">Service for authentication related operations.</param>
    /// <param name="cantonService">To get canton related information </param>
    /// <param name="errorStoreFactory"></param>
    public ProfileModel(IDomainService<User, string> userService, IAuthService authService,
        IDomainService<Canton, string> cantonService, IErrorStoreFactory errorStoreFactory)
    {
        _userService = userService;
        _authService = authService;
        _cantonService = cantonService;
        ChangePasswordModalErrors = errorStoreFactory.Create();
        UpdateUserDataModalErrors = errorStoreFactory.Create();
    }


    /// <summary>
    ///     Handler for the GET request to load the user's profile page.
    /// </summary>
    /// <returns>The user's profile page.</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userService.Get(_authService.GetUserId());
        if (user == null) return RedirectToRoute("Account/Login");
        SetProfileViewModel(user);
        RecoverTemporaryFlags();
        ClearStoredErrors();
        return Page();
    }

    /// <summary>
    /// Handles a POST request to change the user's password.
    /// </summary>
    /// <returns>
    /// If the user is not found, redirects to the Login page.
    /// If the current password is incorrect, re-renders the page with error messages
    /// On success, changes the password, refreshes the user login, and redirects to the profile page.
    /// </returns>
    public async Task<IActionResult> OnPostChangePasswordAsync()
    {
        var user = await _userService.Get(_authService.GetUserId());
        if (user == null) return RedirectToRoute("Account/Login");

        if (!await _authService.IsCurrentPasswordCorrect(PasswordChange.CurrentPassword))
        {
            ChangePasswordModalErrors.StoreError("La contraseña actual no es correcta.");
            SetProfileViewModel(user);
            return Page();
        }

        await _authService.ChangePassword(PasswordChange.CurrentPassword, PasswordChange.NewPassword);

        await _authService.RefreshUserLogin();

        StoreTemporaryFlag("IsPasswordChanged", true);
        return RedirectToPage();
    }

    /// <summary>
    /// Handles a POST request to update the user's data.
    /// </summary>
    /// <returns>
    /// If the user is not found, redirects to the Login page.
    /// If the update data is invalid, re-renders the page with an error message.
    /// On success, updates the user data and redirects to the profile page.
    /// </returns>
    public async Task<IActionResult> OnPostUpdateUserDataAsync()
    {
        var user = await _userService.Get(_authService.GetUserId());

        if (user == null) return RedirectToRoute("Account/Login");

        if (!UserDataUpdate.IsUpdateValid())
        {
            UpdateUserDataModalErrors.StoreError("No se han ingresado datos validos para actualizar.");
            UpdateUserDataModalErrors.StoreError(
                "Se debe ingresar al menos un email o una dirección completa (Provincia, Cantón y Dirección Exacta).");
            SetProfileViewModel(user);
            return Page();
        }

        PrepareUserDataUpdate(user);
        await _userService.Update(user);

        StoreTemporaryFlag("IsUserDataUpdated", true);
        return RedirectToPage();
    }
    /// <summary>
    /// Handles ajax GET request to retrieve canton names based on the specified province.
    /// </summary>
    /// <param name="province">The name of the province.</param>
    /// <returns>
    /// A JSON result containing a list of <see cref="CantonDto"/> objects representing the cantons in the specified province.
    /// If the province parameter is null or empty, returns an empty list.
    /// </returns>
    public async Task<JsonResult> OnGetCantonsAsync(string province)
    {
        if (string.IsNullOrEmpty(province)) return new JsonResult(new List<CantonDto>());

        var cantonNames = await GetCantonsOnProvince(province);
        return new JsonResult(cantonNames);
    }

    /// <summary>
    ///     Prepares the user data to be updated.
    /// </summary>
    /// <param name="user"> the user to update </param>
    private void PrepareUserDataUpdate(User user)
    {
        user.Email = UserDataUpdate.IsEmailEmpty() ? user.Email : UserDataUpdate.Email;
        user.NormalizedEmail = user.Email.ToUpper();
        user.Address = UserDataUpdate.IsAddressEmpty() ? user.Address : UserDataUpdate.GetAddress();
    }

    /// <summary>
    ///     Prepare the ProfileViewModel to be displayed on the page.
    /// </summary>
    /// <param name="user">The user whose information will be displayed.</param>
    private void SetProfileViewModel(User user)
    {
        UserProfile = new ProfileViewModel(user);
    }

    /// <summary>
    ///     Stores the status of the user data modification operation in TempData so it can be recovered after a redirect.
    /// </summary>
    /// <param name="key"> the temporary data name</param>
    /// <param name="value"> the value of the boolean flag </param>
    private void StoreTemporaryFlag(string key, bool value)
    {
        TempData[key] = value;
    }

    /// <summary>
    ///     Recovers the status of the user data modification operation from TempData.
    /// </summary>
    /// <param name="key"> the name of the temporary boolean flag</param>
    /// <returns></returns>
    private bool RecoverTemporaryFlag(string key)
    {
        var value = TempData[key] as bool?;
        TempData.Remove(key);
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
    ///     Retrieves all the cantons names for a given province.
    /// </summary>
    /// <param name="province"> the name of the province get the cantons from </param>
    /// <returns>a list of Canton Names in that province </returns>
    private async Task<List<CantonDto>> GetCantonsOnProvince(string province)
    {
        var cantons = await _cantonService.GetAll();
        return cantons?
            .Where(c => c.Province.Name == province)
            .Select(c => new CantonDto(c))
            .ToList() ?? new List<CantonDto>();
    }
    /// <summary>
    ///   Clears all the stored error messages.
    /// </summary>
    private void ClearStoredErrors()
    {
        ChangePasswordModalErrors.ClearStore();
        UpdateUserDataModalErrors.ClearStore();
    }
}