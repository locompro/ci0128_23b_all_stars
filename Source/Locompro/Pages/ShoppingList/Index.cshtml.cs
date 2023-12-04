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

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ShoppingListDto shoppingListDto = await _shoppingListService.Get();

            ShoppingListMapper shoppingListMapper = new ShoppingListMapper();

            ShoppingListVm = shoppingListMapper.ToVm(shoppingListDto);

            ShoppingListSummaryDto shoppingListSummaryDto = await _shoppingListService.GetStoreSummary();

            ShoppingListSummaryMapper shoppingListSummaryMapper = new ShoppingListSummaryMapper();

            ShoppingListSummaryVm shoppingListSummaryVm = shoppingListSummaryMapper.ToVm(shoppingListSummaryDto);
        } catch (Exception e)
        {
            Logger.LogError(e, "Error while getting shopping list");
            ShoppingListVm = new ShoppingListVm();
            ShoppingListSummaryVm = new ShoppingListSummaryVm();
        }

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