using Locompro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Locompro.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public User UserM { get; set; }
        public string ConfirmPassword { get; set; }
        public bool AcceptedTerms { get; set; }

        public PageResult OnGet()
        {
            return Page(); // return the page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            return Page(); // Placeholder
        }


    }
}
