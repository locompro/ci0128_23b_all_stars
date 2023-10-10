using Locompro.Models.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Submissions;

/// <summary>
/// Page model for the submission creation page.
/// </summary>
public class CreateModel : PageModel
{
    [BindProperty]
    public StoreViewModel StoreVm { get; set; }
    
    [BindProperty]
    public ProductViewModel ProductVm { get; set; }
}