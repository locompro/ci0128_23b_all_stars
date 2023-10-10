using System.ComponentModel.DataAnnotations;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

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
    
    [BindProperty]
    [Required(ErrorMessage = "Ingresar el precio del producto.")]
    public int Price { get; set; }

    private readonly StoreService _storeService;

    private readonly ProductService _productService;

    public CreateModel(StoreService storeService, ProductService productService)
    {
        _storeService = storeService;
        _productService = productService;
    }

    public async Task<IActionResult> OnGetFetchStores(string partialName)
    {
        var stores = await _storeService.GetByPartialId(partialName);
        
        var result = stores.Select(s => new 
        {
            id = s.Name, 
            text = $"{s.Name} ({s.Canton.Province.Name}, {s.Canton.Name})"
        }).ToList();
        
        return new JsonResult(result);
    }
    
    public async Task<IActionResult> OnGetFetchProducts(string partialName, string store)
    {
        // TODO: Go through submissions first to limit products by store
        
        var products = await _productService.GetByPartialName(partialName);
        
        var result = products.Select(p => new 
        {
            id = p.Name, 
            text = $"{p.Name} ({p.Brand}, {p.Model})"
        }).ToList();
        
        return new JsonResult(result);
    }
}
