using Locompro.Services;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Locompro.Pages
{
    /// <summary>
    /// Index page model
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// string for search query product name
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Service that handles the advanced search modal
        /// Helps to keep page and modal information syncronized
        /// </summary>
        AdvancedSearchModalService advancedSearchServiceHandler;

        public IndexModel(AdvancedSearchModalService advancedSearchServiceHandler)
        {
            this.advancedSearchServiceHandler = advancedSearchServiceHandler;
        }

        /// <summary>
        /// Returns view component modal for advanced search
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public IActionResult OnGetAdvancedSearch(string searchQuery)
        {
            this.SearchQuery = searchQuery;

            // generate the view component
            var viewComponentResult = ViewComponent("AdvancedSearch", this.advancedSearchServiceHandler);

            // return it for it to be integrated
            return viewComponentResult;
        }

        /// <summary>
        /// Updates the cantons and the province selected in the advanced search modal
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetUpdateProvince(string province)
        {
            // update the model with all cantons in the given province
            await this.advancedSearchServiceHandler.ObtainCantonsAsync(province);

            // prevent the json serializer from looping infinitely
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // generate the json file with the cantons
            var cantonsJson = JsonConvert.SerializeObject(this.advancedSearchServiceHandler.cantons, settings);

            // specify the content type as a json file
            Response.ContentType = "application/json";

            // send to client
            return Content(cantonsJson);
        }
    }
}