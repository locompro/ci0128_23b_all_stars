using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Locompro.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages;

public class TestFrontTableModel : PageModel
{
    private readonly IShoppingListService _shoppingListService;

    public TestFrontTableModel(IShoppingListService shoppingListService)
    {
        //_shoppingListService = shoppingListService;
    }

    public ShoppingList ShoppingList { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        //ShoppingList = await _shoppingListService.GetShoppingList();
        //if (ShoppingList == null)
        //{
        //    return NotFound();
        //}

        return Page();
    }

}