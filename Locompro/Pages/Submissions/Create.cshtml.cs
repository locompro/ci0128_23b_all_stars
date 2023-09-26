using Locompro.Pages.Modals.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Locompro.Pages.Submissions;

public class CreateModel : PageModel
{
    public void OnGet()
    {
        
    }
    
    public PartialViewResult OnGetAddStore(string customerId)
    {
        return new PartialViewResult
        {
            ViewName = "Modals/Stores/Create",
            ViewData = new ViewDataDictionary<StoreCreateModel>(ViewData, new StoreCreateModel())
        };
    }
}

