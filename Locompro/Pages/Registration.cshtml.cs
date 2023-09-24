using Locompro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages
{
    public class RegistrationModel : PageModel
    {
        public User user { get; set; }
        public string passwordConfirm { get; set; }
        public bool acceptedTerms { get; set; }

        public PageResult OnGet()
        {
            return Page(); // return the page
        }


    }
}
