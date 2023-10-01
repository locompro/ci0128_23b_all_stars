using Locompro.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Submissions;

public class CreateModel : PageModel
{
    public StoreViewModel StoreVm { get; set; }
    
    public void OnGet()
    {

    }

    // public PartialViewResult OnGetCreateStore(string customerId) => Partial("Components/Stores/Create");
}
