using Locompro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;

namespace Locompro.Pages;

public class IndexModel : PageModel
{
    public string SearchQuery { get; set; }

    AdministrativeUnitService administrativeUnitService;

    AdvancedSearchService advancedSearchServiceHandler;

    public IndexModel(AdministrativeUnitService administrativeUnitService,
        AdvancedSearchService advancedSearchServiceHandler)
    {
        this.administrativeUnitService = administrativeUnitService;
        this.advancedSearchServiceHandler = advancedSearchServiceHandler;
    }

    public void OnGet()
    {
    }

    public void OnPost()
    {
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
        await this.advancedSearchServiceHandler.fetchCantonsAsync(province);

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