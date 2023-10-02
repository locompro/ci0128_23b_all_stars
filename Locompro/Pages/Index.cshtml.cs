using Locompro.Services;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Locompro.Pages
{
    public class SearchParameters
    {
        public string query { get; set; }
        public string province { get; set; }
        public string canton { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public string category { get; set; }
        public string model { get; set; }
    }
    public class IndexModel : PageModel
    {
        public string SearchQuery { get; set; }

        AdvancedSearchModalService advancedSearchServiceHandler;

        public IndexModel(AdvancedSearchModalService advancedSearchServiceHandler)
        {
            this.advancedSearchServiceHandler = advancedSearchServiceHandler;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
    
        }

        public void OnPostSendSearchParameters([FromBody] SearchParameters searchParameters)
        {
            string query = (string)searchParameters.query;
            string province = (string)searchParameters.province;
            string canton = (string)searchParameters.canton;
            int minValue = (int)searchParameters.minValue;
            int maxValue = (int)searchParameters.maxValue;
            string category = (string)searchParameters.category;
            string model = (string)searchParameters.model;

            RedirectToPage("/SearchResults/SearchResults", new {query, province, canton, minValue, maxValue, category, model });
        }

        public IActionResult OnGetAdvancedSearch(string searchQuery)
        {
            this.SearchQuery = searchQuery;

            // generate the view component
            var viewComponentResult = ViewComponent("AdvancedSearch", this.advancedSearchServiceHandler);

            // return it for it to be integrated
            return viewComponentResult;
        }

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