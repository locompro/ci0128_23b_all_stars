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
    public ShoppingListVm ShoppingListVm { get; set; }
    
    public ShoppingListSummaryVm ShoppingListSummaryVm { get; set; }

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

            ShoppingListVm = shoppingListMapper.ToVm(shoppingListDto);

            ShoppingListSummaryDto shoppingListSummaryDto = await _shoppingListService.GetSummary();

            ShoppingListSummaryMapper shoppingListSummaryMapper = new ShoppingListSummaryMapper();

            ShoppingListSummaryVm = shoppingListSummaryMapper.ToVm(shoppingListSummaryDto);
        } catch (Exception e)
        {
            Logger.LogError(e, "Error while getting shopping list");
            ShoppingListVm = new ShoppingListVm();
            ShoppingListSummaryVm = new ShoppingListSummaryVm();
        }

        return Page();
    }

    /// <summary>
    /// On POST, adds a product by ID to the current user's shopping list
    /// </summary>
    /// <param name="productId">ID for the product to add to the user's shopping list</param>
    /// <returns>Json result for success or failure</returns>
    public async Task<IActionResult> OnPostAddProduct(int productId)
    {
        try
        {
            await _shoppingListService.AddProduct(productId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while adding product to shopping list");
            return new JsonResult(new { success = false, message = "Error while adding product to shopping list" });
        }
        
        return new JsonResult(new { success = true, message = "Product successfully added to shopping list" });
    }

    /// <summary>
    /// On POST, deletes a product by ID from the current user's shopping list
    /// </summary>
    /// <param name="productId">ID for the product to delete from the user's shopping list</param>
    /// <returns>Json result for success or failure</returns>
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