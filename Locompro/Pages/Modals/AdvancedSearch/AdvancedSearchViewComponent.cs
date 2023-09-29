using Microsoft.AspNetCore.Mvc;
using Locompro.Services;

namespace Locompro.Pages.Modals.AdvancedSearchViewComponent
{
    public class AdvancedSearchViewComponent : ViewComponent
    {
        // model to be connected to page
        private AdvancedSearchService advancedSearchServiceHandler;

        public AdvancedSearchViewComponent(AdvancedSearchService advancedSearchServiceHandler)
        {
            // get the handler
            this.advancedSearchServiceHandler = advancedSearchServiceHandler;

            // fetch all the provinces
            this.advancedSearchServiceHandler.fetchProvincesAsync().Wait();
            // fetch all the cantons for the first province shown
            this.advancedSearchServiceHandler.fetchCantonsAsync(
                    this.advancedSearchServiceHandler.provinces[0].Name).Wait();
        }

        // function to return the view Component with the model
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await this.OnGetAsync());
        }

        // function to return the model
        public async Task<AdvancedSearchService> OnGetAsync()
        {
            return this.advancedSearchServiceHandler;
        }
    }
}
