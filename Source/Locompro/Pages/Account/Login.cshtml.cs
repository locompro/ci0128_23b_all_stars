// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Locompro.Services;
using Locompro.Models.ViewModels;

namespace Locompro.Pages.Account
{
    /// <summary>
    /// Provides the model logic for user login on the Login page.
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly AuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        public LoginModel(AuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Gets or sets the login input model.
        /// </summary>
        [BindProperty]
        public LoginViewModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the error message to be displayed.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Handles the HTTP GET request for the Login page.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after a successful login, if any.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ReturnUrl = returnUrl;
        }
        /// <summary>
        /// Handles the HTTP POST request for the Login page when the user submits the login form.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after a successful login, if any.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");


            if (ModelState.IsValid)
            {
                
                var result = await authService.Login(Input);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Datos incorrectos de inicio");
                    return Page();
                }
            }

            return Page();
        }
    }
}