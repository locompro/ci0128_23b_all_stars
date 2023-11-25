using System.Security.Claims;
using Locompro.Common;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Pages.Util;
using Locompro.Services;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Locompro.Pages.Submissions;

/// <summary>
///     Page model for the submission creation page.
/// </summary>
[Authorize]
public class CreateModel : PageModel
{
    private readonly IContributionService _contributionService;
    
    private readonly IApiKeyHandler _apiKeyHandler;

    private readonly INamedEntityDomainService<Product, int> _productService;

    private readonly INamedEntityDomainService<Store, string> _storeService;

    public CreateModel(INamedEntityDomainService<Store, string> storeService,
        INamedEntityDomainService<Product, int> productService,
        IContributionService contributionService,
        IApiKeyHandler apiKeyHandler)
    {
        _storeService = storeService;
        _productService = productService;
        _contributionService = contributionService;
        _apiKeyHandler = apiKeyHandler;
        
    }
    
    [BindProperty] public StoreVm StoreVm { get; set; }

    [BindProperty] public ProductVm ProductVm { get; set; }

    [BindProperty] public SubmissionVm SubmissionVm { get; set; }

    public async Task<IActionResult> OnGetFetchStores(string partialName)
    {
        var stores = await _storeService.GetByPartialName(partialName);

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
    public IActionResult OnGetGoogleMapsApiKey()
    {
        var apiKey = _apiKeyHandler.GetApiKey();
        return new JsonResult(apiKey);
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
        
        if (!ModelState.IsValid) return Page();

        SubmissionVm.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var formFiles = Request.Form.Files;
        
        var pictureVMs = PictureParser.Parse(formFiles);
        
        await _contributionService.AddSubmission(StoreVm, ProductVm, SubmissionVm, pictureVMs);
        
        return RedirectToPage("/Index");
    }
}