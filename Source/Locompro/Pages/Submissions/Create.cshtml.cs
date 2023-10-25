using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Locompro.Pages.Submissions;

/// <summary>
/// Page model for the submission creation page.
/// </summary>
[Authorize]
public class CreateModel : PageModel
{
    [BindProperty]
    public StoreViewModel StoreVm { get; set; }
    
    [BindProperty]
    public ProductViewModel ProductVm { get; set; }
    
    [BindProperty]
    [StringLength(120)]
    public string Description { get; set; }
    
    [BindProperty]
    [Required(ErrorMessage = "Ingresar el precio del producto.")]
    [Range(100, 10000000, ErrorMessage = "El precio debe estar entre ₡100 y ₡10.000.000.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El precio debe contener solamente números enteros.")]
    public int? Price { get; set; }

    private readonly StoreService _storeService;

    private readonly ProductService _productService;
    
    private readonly ContributionService _contributionService;

    private readonly UserManager<User> _userManager;

    public CreateModel(StoreService storeService, ProductService productService, ContributionService contributionService, UserManager<User> userManager)
    {
        _storeService = storeService;
        _productService = productService;
        _contributionService = contributionService;
        _userManager = userManager;
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
        
        // TODO: Discern active vs. inactive
        
        var products = await _productService.GetByPartialName(partialName);
        
        var result = products.Select(p => new 
        {
            id = p.Id, 
            text = $"{p.Name} ({p.Brand}, {p.Model})"
        }).ToList();
        
        return new JsonResult(result);
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        // Remove ModelState errors for StoreVm if it's an existing store
        if (StoreVm != null && StoreVm.IsExistingStore())
        {
            ModelState.Remove("StoreVm.Address");
            ModelState.Remove("StoreVm.Telephone");
            ModelState.Remove("StoreVm.Province");
            ModelState.Remove("StoreVm.Canton");
        }
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _contributionService.AddSubmission(StoreVm, ProductVm, Description, Price.GetValueOrDefault(), userId);

        return RedirectToPage("/Index");
    }
}
