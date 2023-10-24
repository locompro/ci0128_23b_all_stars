using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Locompro.Services;
using Castle.Core.Internal;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Common;
using Locompro.Models;
using Microsoft.Extensions.Configuration;
using Locompro.Repositories.Utilities;

namespace Locompro.Pages.SearchResults;

/// <summary>
/// Page model for the search results page
/// </summary>
public class SearchResultsModel : PageModel
{
    /// <summary>
    /// Service that handles the advanced search modal
    /// </summary>
    private readonly AdvancedSearchInputService _advancedSearchServiceHandler;

    private readonly SearchService _searchService;

    /// <summary>
    /// Buffer for page size according to Paginated List and configuration
    /// </summary>
    private readonly int _pageSize;

    /// <summary>
    /// Paginated list of products found
    /// </summary>
    public PaginatedList<Item> DisplayItems { get; set; }

    /// <summary>
    /// List of all items found
    /// </summary>
    private List<Item> _items;

    /// <summary>
    /// Amount of items found
    /// </summary>
    public double ItemsAmount { get; set; }

    /// <summary>
    /// Name of product that was searched
    /// </summary>
    public string ProductName { get; set; }

    public string ProvinceSelected { get; set; }
    public string CantonSelected { get; set; }
    public string CategorySelected { get; set; }
    public long MinPrice { get; set; }
    public long MaxPrice { get; set; }
    public string ModelSelected { get; set; }
    public string BrandSelected { get; set; }
    public string CurrentFilter { get; set; }
        
    public string NameSort { get; set; }
        
    public string CurrentSort { get; set; }
    public string CantonSort { get; set; }
    public string ProvinceSort { get; set; }
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="searchService"></param>
    /// <param name="advancedSearchServiceHandler"></param>
    /// <param name="configuration"></param>
    public SearchResultsModel(
        AdvancedSearchInputService advancedSearchServiceHandler,
        IConfiguration configuration,
        SearchService searchService)
    {
        _searchService = searchService;
        _advancedSearchServiceHandler = advancedSearchServiceHandler;
        _pageSize = configuration.GetValue("PageSize", 4);
    }

    /// <summary>
    /// Gets the items to be displayed in the search results
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="sorting"></param>
    /// <param name="query"></param>
    /// <param name="province"></param>
    /// <param name="canton"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="category"></param>
    /// <param name="model"></param>
    /// <param name="brand"></param>
    /// <param name="currentFilter"></param>
    /// <param name="sortOrder"></param>
    public async Task OnGetAsync(int? pageIndex,
        bool? sorting,
        string query,
        string province,
        string canton,
        long minValue,
        long maxValue,
        string category,
        string model,
        string brand,
        string currentFilter,
        string sortOrder)
    {
       
        // validate input
        ValidateInput(province, canton, minValue, maxValue, category, model, brand);
        
        ProductName = query;
        
        // set up sorting parameters
        SetSortingParameters(sortOrder, (sorting is not null));
        
        // get items from search service
        List<SearchCriterion> searchParameters = new List<SearchCriterion>()
        {
            new (SearchParam.SearchParameterTypes.Name, ProductName),
            new (SearchParam.SearchParameterTypes.Province, ProvinceSelected),
            new (SearchParam.SearchParameterTypes.Canton, CantonSelected),
            new (SearchParam.SearchParameterTypes.Minvalue, MinPrice.ToString()),
            new (SearchParam.SearchParameterTypes.Maxvalue, MaxPrice.ToString()),
            new (SearchParam.SearchParameterTypes.Category, CategorySelected),
            new (SearchParam.SearchParameterTypes.Model, ModelSelected),
            new (SearchParam.SearchParameterTypes.Brand, BrandSelected),
        };

        _items = await this._searchService.GetSearchResults(searchParameters);
        
        // get amount of items found    
        ItemsAmount = _items.Count;
        
        // order items by sort order
        OrderItems();
        
        // create paginated list and set it to be displayed
        this.DisplayItems = PaginatedList<Item>.Create(_items, pageIndex ?? 1, _pageSize);
    }

    /// <summary>
    /// Changes input from front end into values usable for search engine
    /// </summary>
    /// <param name="province"></param>
    /// <param name="canton"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="category"></param>
    /// <param name="model"></param>
    /// <param name="brand"></param>
    private void ValidateInput(
        string province,
        string canton,
        long minValue,
        long maxValue,
        string category,
        string model,
        string brand)
    {
        if (!string.IsNullOrEmpty(province) && province.Equals("Ninguno"))
        {
            province = null;
        }
        
        if (!string.IsNullOrEmpty(canton) && canton.Equals("Ninguno"))
        {
            canton = null;
        }

        if (!string.IsNullOrEmpty(category) && category.Equals("Ninguno"))
        {
            category = null;
        } 
        
       ProvinceSelected = province;
       CantonSelected = canton;
       MinPrice = minValue;
       MaxPrice = maxValue;
       CategorySelected = category;
       ModelSelected = model;
       BrandSelected = brand;
    }

    /// <summary>
    /// Manages all sorting done to items in list
    /// </summary>
    /// <param name="sortOrder"></param>
    /// <param name="sorting"></param>
    private void SetSortingParameters(string sortOrder, bool sorting)
    {
        if (!sorting && !string.IsNullOrEmpty(sortOrder))
        {
            CurrentSort = NameSort = sortOrder;
            return;
        }

        // Define a dictionary to hold the reverse sortOrder mappings
        Dictionary<string, string> sortMappings = new Dictionary<string, string>
        {
            { "name_asc", "name_desc" },
            { "name_desc", "name_asc" },
            { "province_asc", "province_desc" },
            { "province_desc", "province_asc" },
            { "canton_asc", "canton_desc" },
            { "canton_desc", "canton_asc" },
        };

        // If sortOrder is null or not in the dictionary, set default values
        if (string.IsNullOrEmpty(sortOrder) || !sortMappings.ContainsKey(sortOrder))
        {
            CurrentSort = NameSort = "name_desc";
            ProvinceSort = "province_asc";
            CantonSort = "canton_asc";
            return;
        }

        // Update sorting based on the mapping and set CurrentSort
        switch (sortOrder)
        {
            case "name_asc":
            case "name_desc":
                CurrentSort = NameSort = sortMappings[sortOrder];
                break;
            case "province_asc":
            case "province_desc":
                CurrentSort = ProvinceSort = sortMappings[sortOrder];
                break;
            case "canton_asc":
            case "canton_desc":
                CurrentSort = CantonSort = sortMappings[sortOrder];
                break;
        }
    }




    /// <summary>
    /// Orders items
    /// </summary>
    void OrderItems()
    {
        switch (NameSort)
        {
            case "name_desc":
                _items = _items.OrderByDescending(item => item.Name).ToList();
                break;
            case "name_asc":
                _items = _items.OrderBy(item => item.Name).ToList();
                break;
        }

        if(!string.IsNullOrEmpty(ProvinceSort))
        {
            switch (ProvinceSort)
            {
                case "province_desc":
                    _items = _items.OrderByDescending(item => item.Province).ToList();
                    break;
                case "province_asc":
                    _items = _items.OrderBy(item => item.Province).ToList();
                    break;
            }
        }

        if(!string.IsNullOrEmpty(CantonSort))
        {
            switch (CantonSort)
            {
                case "canton_desc":
                    _items = _items.OrderByDescending(item => item.Canton).ToList();
                    break;
                case "canton_asc":
                    _items = _items.OrderBy(item => item.Canton).ToList();
                    break;
            }
        }
    }
    
    /// <summary>
    /// Returns the view component for the advanced search modal
    /// </summary>
    /// <returns></returns>
    public IActionResult OnGetAdvancedSearch()
    {
        // generate the view component
        var viewComponentResult = ViewComponent("AdvancedSearch", this._advancedSearchServiceHandler);

        // return it for it to be integrated
        return viewComponentResult;
    }

    /// <summary>
    /// Updates the cantons and province selected for the advanced search modal
    /// </summary>
    /// <param name="province"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnGetUpdateProvince(string province)
    {
        string cantonsJson = "";

        // if province is none
        if (province.Equals("Ninguno"))
        {
            // create empty list
            List<Canton> emptyCantonList = new List<Canton>
            {
                // add none back as an option
                new Canton{CountryName = "Ninguno",
                    Name = "Ninguno", 
                    ProvinceName = "Ninguno"}
            };

            // set new list to service canton list
            _advancedSearchServiceHandler.Cantons = emptyCantonList;
        }
        else
        {
            // update the model with all cantons in the given province
            await _advancedSearchServiceHandler.ObtainCantonsAsync(province);

            _advancedSearchServiceHandler.Cantons.Add(
                new Canton{CountryName = "Ninguno",
                    Name = "Ninguno", 
                    ProvinceName = "Ninguno"}
            );

            foreach (var canton in _advancedSearchServiceHandler.Cantons)
            {
                Console.WriteLine(canton.Name);
            }
        }

        // prevent the json serializer from looping infinitely
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        // generate the json file with the cantons
        cantonsJson = JsonConvert.SerializeObject(_advancedSearchServiceHandler.Cantons, settings);

        // specify the content type as a json file
        Response.ContentType = "application/json";

        // send to client
        return Content(cantonsJson);
    }
}