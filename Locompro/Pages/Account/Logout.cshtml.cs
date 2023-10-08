// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using Locompro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Account;

/// <summary>
/// Provides the model logic for user logout on the Logout page.
/// </summary>
public class LogoutModel : PageModel
{
    private readonly AuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutModel"/> class.
    /// </summary>
    /// <param name="authService">The service to handle user authentication.</param>
    public LogoutModel(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Handles the HTTP POST request for the Logout page when the user submits the logout request.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. Redirects to the index page upon successful logout.</returns>
    public async Task<IActionResult> OnPost()
    {
        await _authService.Logout();
        return RedirectToPage("/Index");
    }
}