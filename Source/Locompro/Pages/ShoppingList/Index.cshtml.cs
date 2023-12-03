using Locompro.Models.ViewModels;
using Locompro.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Pages.ShoppingList;

public class ShoppingListModel : BasePageModel
{
    public ShoppingListVm ShoppingListVm;
    
    public StoreSummaryVm StoreSummaryVm;
    
    public ShoppingListModel(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor) : base(loggerFactory, httpContextAccessor)
    {
    }

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnGetStoreSummaryAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAddProduct(int productId)
    {
        return new JsonResult(new { success = true, message = "Product successfully added to shopping list" });
    }
    
    public async Task<IActionResult> OnPostDeleteProduct(int productId)
    {
        return new JsonResult(new { success = true, message = "Product successfully deleted from shopping list" });
    }
}