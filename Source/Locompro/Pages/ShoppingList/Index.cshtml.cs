using Locompro.Common.Mappers;
using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;
using Locompro.Pages.Shared;
using Locompro.Services;
using Locompro.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Pages.ShoppingList;

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

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ShoppingListDto shoppingListDto = await _shoppingListService.Get();
        
            ShoppingListMapper mapper = new ShoppingListMapper();

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

    public async Task<IActionResult> OnGetStoreSummaryAsync()
    {
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