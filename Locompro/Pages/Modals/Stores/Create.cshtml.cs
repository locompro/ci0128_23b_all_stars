using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Modals.Stores;

public class CreateModel : PageModel
{
    public string Message { get; set; } = "Hello from Modal Partial";

    public void OnGet()
    {
    }

    public void OnPost()
    {
        Message = "Form submitted successfully!";
    }
}