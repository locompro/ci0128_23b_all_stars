using Microsoft.AspNetCore.Mvc;
using Locompro.Services;
using Locompro.Pages.Modals.AdvancedSearch;

namespace Locompro.Pages.Modals.AdvancedSearchViewComponent
{
    public class AdvancedSearchViewComponent : ViewComponent
    {
        // model to be connected to page
        private AdvancedSearchModalService advancedSearchServiceHandler;

        public AdvancedSearchModalModel pageModel { get; set; }

        public AdvancedSearchViewComponent(AdvancedSearchModalService advancedSearchServiceHandler)
        {
            // get the handler
            this.advancedSearchServiceHandler = advancedSearchServiceHandler;

            this.pageModel = new AdvancedSearchModalModel(this.advancedSearchServiceHandler);

            // get all the provinces
            this.pageModel.ObtainProvincesAsync().Wait();

            // get all the cantons for the first province shown
            this.pageModel.ObtainCantonsAsync(
                    this.advancedSearchServiceHandler.provinces[0].Name).Wait();

            this.pageModel.ObtainCategoriesAsync();
        }

        // function to return the view Component with the model
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await this.OnGetAsync());
        }

        // function to return the model
        public async Task<AdvancedSearchModalModel> OnGetAsync()
        {
            return await Task.FromResult(this.pageModel) ;
        }
    }
}
