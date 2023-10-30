﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Threading.Tasks;
using Locompro.Areas.Identity.ViewModels;
using Locompro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService authService;

        public RegisterModel(IAuthService authService)
        {
            this.authService = authService;
        }

        [BindProperty] public RegisterViewModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Acts over a post request to the register page. Uses the AuthService to register the user.
        /// </summary> 
        /// <param name="returnUrl">Url to return to after registering.</param>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            _ = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var registerSuccess = await authService.Register(Input);
                if (registerSuccess.Succeeded)
                {
                    return RedirectToPage("/Index");
                }

                foreach (var error in registerSuccess.Errors)
                {
                    ModelState.AddModelError(string.Empty, "El usuario ya existe");
                }
            }

            Console.WriteLine("Model state is not valid");
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}