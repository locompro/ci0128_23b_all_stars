// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Locompro.Areas.Identity.ViewModels;
using Locompro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserService userService;

        public RegisterModel(UserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            _ = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
            {
                var registerSuccess = await userService.Register(Input);
                if (registerSuccess.Succeeded)
                {
                    return RedirectToPage("/Index");
                }

                foreach (var error in registerSuccess.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
