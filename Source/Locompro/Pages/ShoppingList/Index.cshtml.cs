using Locompro.Common.Mappers;
using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;
using Locompro.Pages.Shared;
using Locompro.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Pages.ShoppingList;

/// <summary>
/// Page model for the shopping list page
/// </summary>
public class ShoppingListModel : BasePageModel
{
    public ShoppingListVm ShoppingList { get; set; }

    public StoreSummaryVm StoreSummaryVm { get; set; }
    
    private readonly IShoppingListService _shoppingListService;

    public ShoppingListModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        IShoppingListService shoppingListService)
        : base(loggerFactory, httpContextAccessor)
    {
        _shoppingListService = shoppingListService;
    }

    /// <summary>
    /// On GET, loads all shopping list data into page model
    /// </summary>
    /// <returns>Page render result</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ShoppingListDto shoppingListDto = await _shoppingListService.Get();

            ShoppingListMapper shoppingListMapper = new ShoppingListMapper();

            ShoppingList = mapper.ToVm(shoppingListDto);
            Logger.LogInformation("ShoppingList UserId:" + ShoppingList.UserId );
            Logger.LogInformation("ShoppingList ProductCount:" + ShoppingList.Products.Count);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while getting shopping list");
            ShoppingList = new ShoppingListVm();
        }
        
        return Page();
    }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteProduct(int productId)
    {
        try
        {
            await _shoppingListService.DeleteProduct(productId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while deleting product from shopping list");
            return new JsonResult(new { success = false, message = "Error while deleting product from shopping list" });
        }
        
        return new JsonResult(new { success = true, message = "Product successfully deleted from shopping list" });
    }
}