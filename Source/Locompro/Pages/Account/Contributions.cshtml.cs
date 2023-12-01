using Castle.Core.Internal;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services.Auth;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Locompro.Pages.Account
{
    public class ContributionsPageModel : PageModel
    {
        private readonly IUserManagerService _userManagerService;
        private IConfiguration Configuration { get; set; }

        public readonly int PageSize;
        public string RequestedUserId { get; set; }
        public ContributionsVm RequestedUser { get; set; }
        
        public string ContributionsToShow { get; set; }
        public ContributionsPageModel(IUserManagerService userManagerService, 
            IConfiguration configuration)
        {
            _userManagerService = userManagerService;
            Configuration = configuration;
            PageSize = 8;
        }

        // Reacts to the userId given, checks their user data and gives all the submissions done by them
        public async Task OnGetAsync(string query)
        {
            RequestedUserId = query;
            var requestedUser = await GetUserRequested(RequestedUserId);
            if (requestedUser != null && requestedUser.CreatedSubmissions != null)
            {
                RequestedUser = new ContributionsVm(requestedUser);
                ContributionsToShow = JsonConvert.SerializeObject(RequestedUser.Contributions);
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
}