using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Locompro.Pages.Shared;

public abstract class SearchPageModel : PageModel
{
    /// <summary>
    /// string for search query product name
    /// </summary>
    public string SearchQuery { get; set; }
    
    /// <summary>
    /// Service that handles the advanced search modal
    /// Helps to keep page and modal information synchronized
    /// </summary>
    private readonly AdvancedSearchInputService _advancedSearchServiceHandler;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public const string EmptyValue = "Todos";

    protected SearchPageModel(AdvancedSearchInputService advancedSearchServiceHandler,
        IHttpContextAccessor httpContextAccessor)
    {
        _advancedSearchServiceHandler = advancedSearchServiceHandler;
        _advancedSearchServiceHandler.EmptyValue = EmptyValue;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<IActionResult> OnPostReturnResultsAsync()
    {
        Console.WriteLine("OnPostReturnResultsAsync");
        
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
            
        SearchViewModel searchViewModel = JsonConvert.DeserializeObject<SearchViewModel>(json);
        
        CacheDataInSession(searchViewModel, "SearchQueryViewModel");
        
        return RedirectToPage("/SearchResults/SearchResults");
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
        var viewComponentResult = ViewComponent("AdvancedSearch", this._advancedSearchServiceHandler);

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
        if (province.Equals(EmptyValue))
        {
            UpdateCantonsOnNoProvince();
        }
        else
        {
            await UpdateCantons(province);
        }

        string cantonsJson = GetCantonsJson();

        // send to client
        return Content(cantonsJson);
    }

    /// <summary>
    /// Updates internal list of cantons according to the selected province
    /// </summary>
    /// <param name="province"> province whose cantons are to be added to internal list</param>
    private async Task UpdateCantons(string province)
    {
        await _advancedSearchServiceHandler.ObtainCantonsAsync(province);

        _advancedSearchServiceHandler.Cantons.Add(
            new Canton{CountryName = EmptyValue,
                Name = EmptyValue, 
                ProvinceName = EmptyValue}
        );
    }
    
    /// <summary>
    /// Updates internal list of cantons to have a empty canton value according to constant EmptyValue
    /// </summary>
    private void UpdateCantonsOnNoProvince()
    {
        List<Canton> emptyCantonList = new List<Canton>
        {
            new Canton{CountryName = EmptyValue,
                Name = EmptyValue, 
                ProvinceName = EmptyValue}
        };
        
        _advancedSearchServiceHandler.Cantons = emptyCantonList;
    }
    
    /// <summary>
    /// Provides a serialized Json string of the cantons in the advanced search modal
    /// </summary>
    /// <returns> the serialized Json </returns>
    private string GetCantonsJson()
    {
        return GetJsonFrom(_advancedSearchServiceHandler.Cantons);
    }

    protected string GetJsonFrom<T>(T objectToSerialize)
    {
        // prevent the json serializer from looping infinitely
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        
        string json = JsonConvert.SerializeObject(objectToSerialize, settings);
        
        Response.ContentType = "application/json";

        return json;
    }
    
    /// <summary>
    /// Stores the given data in a local user session specific instance
    /// </summary>
    /// <param name="objectToCache"> what is to be stored </param>
    /// <param name="key"> identifier to retrieve cached data </param>
    /// <remarks> if for some reason there is no session, one will be instantiated</remarks>
    protected void CacheDataInSession<T>(T objectToCache, string key)
    {
        if (_httpContextAccessor.HttpContext?.Session == null)
        {
            _httpContextAccessor.HttpContext?.RequestServices.GetService(typeof(ISession));
        }
        
        HttpContext.Session.SetString(key, JsonConvert.SerializeObject(objectToCache));
    }
    
    /// <summary>
    /// Retrieves the cached data from the local user session specific instance
    /// </summary>
    /// <param name="key"> identifier to retrieve cached data </param>
    /// <param name="deletesData"> if the data and session are to be deleted </param>
    /// <typeparam name="T"> the type of the data to be retrieved</typeparam>
    /// <remarks> if deletesData is true, the session is removed, but one is produced
    /// by framework, or on a caching action if not automatic </remarks>
    /// <returns> cached data according to key</returns>
    protected T GetCachedDataFromSession<T>(string key, bool deletesData = true)
    {
        if (key == null || HttpContext.Session.GetString(key) == null) return default;
        
        string cachedObjectJson = HttpContext.Session.GetString(key);

        if (cachedObjectJson == null) return default;
        
        T cachedObject = JsonConvert.DeserializeObject<T>(cachedObjectJson);
        
        if (deletesData)
        {
            HttpContext.Session.Remove(key);
            _httpContextAccessor.HttpContext?.Session.Clear();
        }
        
        return cachedObject;
    }
   
}