using Locompro.Common;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locompro.Pages.Shared;

public abstract class SearchPageModel : BasePageModel
{
    public const string EmptyValue = "Todos";

    /// <summary>
    ///     Service that handles the advanced search modal
    ///     Helps to keep page and modal information synchronized
    /// </summary>
    private readonly AdvancedSearchInputService _advancedSearchServiceHandler;
    
    private readonly IApiKeyHandler _apiKeyHandler;

    protected SearchPageModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        AdvancedSearchInputService advancedSearchServiceHandler,
        IApiKeyHandler apiKeyHandler)
        : base(loggerFactory, httpContextAccessor)
    {
        _advancedSearchServiceHandler = advancedSearchServiceHandler;
        _advancedSearchServiceHandler.EmptyValue = EmptyValue;
        _apiKeyHandler = apiKeyHandler;
    }

    /// <summary>
    ///     string for search query product name
    /// </summary>
    public string SearchQuery { get; set; }

    /// <summary>
    ///     Stores locally the data sent by the client, it is expected for the client to immediately request again for this
    ///     data
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostReturnResultsAsync()
    {
        var searchVm = await GetDataSentByClient<SearchVm>();

        CacheDataInSession(searchVm, "SearchQueryViewModel");

        return RedirectToPage("/SearchResults/SearchResults");
    }

    /// <summary>
    ///     Returns view component modal for advanced search
    /// </summary>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    public IActionResult OnGetAdvancedSearch(string searchQuery)
    {
        SearchQuery = searchQuery;

        // generate the view component
        var viewComponentResult = ViewComponent("AdvancedSearch", _advancedSearchServiceHandler);

        // return it for it to be integrated
        return viewComponentResult;
    }

    /// <summary>
    ///     Updates the cantons and the province selected in the advanced search modal
    /// </summary>
    /// <param name="province"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnGetUpdateProvince(string province)
    {
        if (province.Equals(EmptyValue))
            UpdateCantonsOnNoProvince();
        else
            await UpdateCantons(province);

        var cantonsJson = GetCantonsJson();

        // send to client
        return Content(cantonsJson);
    }
    
    /// <summary>
    /// Returns the api key for google maps
    /// </summary>
    /// <returns> api key </returns>
    public IActionResult OnGetReturnMapsApiKey()
    {
        string apiKey = _apiKeyHandler.GetApiKey();
        string apiKeyJson = GetJsonFrom(apiKey);
        return Content(apiKeyJson);
    }

    /// <summary>
    ///     Updates internal list of cantons according to the selected province
    /// </summary>
    /// <param name="province"> province whose cantons are to be added to internal list</param>
    private async Task UpdateCantons(string province)
    {
        await _advancedSearchServiceHandler.ObtainCantonsAsync(province);

        _advancedSearchServiceHandler.Cantons.Add(
            new Canton
            {
                CountryName = EmptyValue,
                Name = EmptyValue,
                ProvinceName = EmptyValue
            }
        );
    }

    /// <summary>
    ///     Updates internal list of cantons to have a empty canton value according to constant EmptyValue
    /// </summary>
    private void UpdateCantonsOnNoProvince()
    {
        var emptyCantonList = new List<Canton>
        {
            new()
            {
                CountryName = EmptyValue,
                Name = EmptyValue,
                ProvinceName = EmptyValue
            }
        };

        _advancedSearchServiceHandler.Cantons = emptyCantonList;
    }

    /// <summary>
    ///     Provides a serialized Json string of the cantons in the advanced search modal
    /// </summary>
    /// <returns> the serialized Json </returns>
    private string GetCantonsJson()
    {
        return GetJsonFrom(_advancedSearchServiceHandler.Cantons);
    }
}