using Microsoft.AspNetCore.Mvc;

using Locompro.Models;
using Locompro.Services;

namespace Locompro.Pages.Shared.Components.AddStore
{
    public class AddStore : ViewComponent
    {
        private readonly CountryService _countryService;

        public AddStore(CountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(StoreViewModel storeVm)
        {
            var country = await _countryService.Get("Costa Rica");
            ViewData["Provinces"] = country.Provinces;
            return View(storeVm);
        }
    }   
}