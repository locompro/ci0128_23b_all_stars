using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services.Auth;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Account;

public class ShoppingListPageModel : PageModel
{
    private readonly IUserManagerService _userManagerService;
    public readonly int PageSize;
    public string RequestedUserId { get; set; }
    public ContributionsVm RequestedUser { get; set; }
        
    public ShoppingListPageModel(IUserManagerService userManagerService, 
        IConfiguration configuration)
    {
        _userManagerService = userManagerService;
        PageSize = configuration.GetValue("PageSize", 4);
    }

    // Reacts to the userId given, checks their user data and gives all the submissions done by them
    public async Task OnGetAsync(int? pageIndex)
    {
        var requestedUser = await GetUserRequested("1");
        if (requestedUser != null)
        {
            RequestedUser = new ContributionsVm (requestedUser);
            var contributionsData = Newtonsoft.Json.JsonConvert.SerializeObject(RequestedUser.Contributions);
            ViewData["ContributionsData"] = contributionsData;
        }
    }
        
    /// <summary>
    ///     Asynchronously retrieves the current user.
    /// </summary>
    /// <returns> the current user </returns>
    private async Task<User> GetUserRequested(string userIdRequested)
    {
        return await _userManagerService.FindByIdAsync(userIdRequested);
    }

}