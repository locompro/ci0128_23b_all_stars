// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Threading.Tasks;
using Locompro.Models;
using Locompro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Locompro.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly AuthService _authService;
    public LogoutModel(AuthService authService)
    {
        _authService = authService;
    }

    public async Task<IActionResult> OnPost()
    {
        await _authService.Logout();
        return RedirectToPage("/Index");
    }
}