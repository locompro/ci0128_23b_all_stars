using Locompro.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Pages.Modals.AdvancedSearch
{
    /// <summary>
    /// View component for the advanced search modal
    /// </summary>
    public class AdvancedSearchViewComponent : ViewComponent
    {
        // model to be connected to page
        private AdvancedSearchInputService advancedSearchServiceHandler;

        public AdvancedSearchModalModel pageModel { get; set; }

        public AdvancedSearchViewComponent(AdvancedSearchInputService advancedSearchServiceHandler)
        {
            // get the handler
            this.advancedSearchServiceHandler = advancedSearchServiceHandler;

            this.pageModel = new AdvancedSearchModalModel(this.advancedSearchServiceHandler);

            // get all the provinces
            this.pageModel.ObtainProvincesAsync().Wait();

            // get all the cantons for the first province shown
            this.pageModel.ObtainCantonsAsync(
                    this.advancedSearchServiceHandler.Provinces[0].Name).Wait();

            this.pageModel.ObtainCategoriesAsync().Wait();
        }

        /// <summary>
        /// function to return the view Component with the model
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await this.OnGetAsync());
        }

        /// <summary>
        /// function to return the model
        /// </summary>
        /// <returns></returns>
        public async Task<AdvancedSearchModalModel> OnGetAsync()
        {
            return await Task.FromResult(this.pageModel) ;
        }
    }
}
