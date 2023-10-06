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
using Locompro.Models;
using Microsoft.Extensions.Configuration;

namespace Locompro.Pages.SearchResults;

/// <summary>
/// Page model for the search results page
/// </summary>
public class SearchResultsModel : PageModel
{
    /// <summary>
    /// Service that handles the advanced search modal
    /// </summary>
    private readonly AdvancedSearchModalService _advancedSearchServiceHandler;

    private readonly SearchService _searchService;

    /// <summary>
    /// Configuration to get the page size
    /// </summary>
    private readonly IConfiguration _configuration;

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
        
        
    public string NameSort { get; set; }
        
    public string CurrentSort { get; set; }
        
    public string CurrentFilter { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="searchService"></param>
    /// <param name="advancedSearchServiceHandler"></param>
    /// <param name="configuration"></param>
    public SearchResultsModel(
        AdvancedSearchModalService advancedSearchServiceHandler,
        IConfiguration configuration,
        SearchService searchService)
    {
        this._searchService = searchService;
        this._advancedSearchServiceHandler = advancedSearchServiceHandler;
        this._configuration = configuration;
        this._pageSize = _configuration.GetValue("PageSize", 4);
    }

    /// <summary>
    /// Gets the items to be displayed in the search results
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="query"></param>
    /// <param name="province"></param>
    /// <param name="canton"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="category"></param>
    /// <param name="model"></param>
    /// <param name="currentFilter"></param>
    /// <param name="sortOrder"></param>
    public async Task OnGetAsync(int? pageIndex,
        string query,
        string province,
        string canton,
        long minValue,
        long maxValue,
        string category,
        string model,
        string currentFilter,
        string sortOrder)
    {
        // validate input
        this.ValidateInput(province, canton, minValue, maxValue, category, model);
        
        this.CurrentSort = sortOrder;
        this.ProductName = query;
        
        // set up sorting parameters
        this.SetSortingParameters(sortOrder);
        
        // get items from search service
        this._items =
            (await _searchService.SearchItems(
                this.ProductName,
                this.ProvinceSelected,
                this.CantonSelected,
                this.MinPrice,
                this.MaxPrice,
                this.CategorySelected,
                this.ModelSelected)
            ).ToList();
        
        // get amount of items found    
        this.ItemsAmount = _items.Count;
            
        // order items by sort order
        this.OrderItems(sortOrder);
        
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
    private void ValidateInput(
        string province,
        string canton,
        long minValue,
        long maxValue,
        string category,
        string model)
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
        
        this.ProvinceSelected = province;
        this.CantonSelected = canton;
        this.MinPrice = minValue;
        this.MaxPrice = maxValue;
        this.CategorySelected = category;
        this.ModelSelected = model;
    }
    
    /// <summary>
    /// Manages all sorting done to items in list
    /// </summary>
    /// <param name="sortOrder"></param>
    private void SetSortingParameters(string sortOrder)
    {
        if (string.IsNullOrEmpty(sortOrder))
        {
            this.NameSort = "name_asc";
        }
        else
        {
            this.NameSort = sortOrder.Contains("name_asc") ? "name_desc" : "name_asc";
        }
    }

    /// <summary>
    /// Orders items
    /// </summary>
    /// <param name="sortOrder"></param>
    void OrderItems(string sortOrder)
    {
        switch (sortOrder)
        {
            case "name_desc":
                _items = _items.OrderByDescending(item => item.ProductName).ToList();
                break;
            case "name_asc":
                _items = _items.OrderBy(item => item.ProductName).ToList();
                break;
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
            this._advancedSearchServiceHandler.Cantons = emptyCantonList;
        }
        else
        {
            // update the model with all cantons in the given province
            await this._advancedSearchServiceHandler.ObtainCantonsAsync(province);
        }
        
        // prevent the json serializer from looping infinitely
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        // generate the json file with the cantons
        cantonsJson = JsonConvert.SerializeObject(this._advancedSearchServiceHandler.Cantons, settings);
        
        // specify the content type as a json file
        Response.ContentType = "application/json";

        // send to client
        return Content(cantonsJson);
    }
}