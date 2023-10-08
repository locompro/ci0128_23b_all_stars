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

namespace Locompro.Pages.SearchResults;

/// <summary>
/// Represents the page model for the search results page, handling the operations related to 
/// displaying and sorting the search results.
/// </summary>
public class SearchResultsModel : PageModel
{
    /// <summary>
    /// Service that handles the advanced search modal
    /// </summary>
    private readonly AdvancedSearchInputService _advancedSearchServiceHandler;

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
    public string BrandSelected { get; set; }


    public string NameSort { get; set; }

    public string CurrentSort { get; set; }
    public string CantonSort { get; set; }
    public string ProvinceSort { get; set; }


    public string CurrentFilter { get; set; }

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
        _configuration = configuration;
        _pageSize = _configuration.GetValue("PageSize", 4);
    }

    /// <summary>
    /// Retrieves and prepares the search results for display on the page.
    /// </summary>
    /// <param name="pageIndex">The index of the page to display.</param>
    /// <param name="sorting">Indicates whether sorting is applied.</param>
    /// <param name="query">The search query.</param>
    /// <param name="province">The selected province for search.</param>
    /// <param name="canton">The selected canton for search.</param>
    /// <param name="minValue">The minimum price filter.</param>
    /// <param name="maxValue">The maximum price filter.</param>
    /// <param name="category">The selected category for search.</param>
    /// <param name="model">The selected model for search.</param>
    /// <param name="brand">The selected brand for search.</param>
    /// <param name="currentFilter">The current filter applied.</param>
    /// <param name="sortOrder">The order in which to sort the results.</param>
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
        SetSortingParameters(sortOrder, sorting is not null);

        // get items from search service
        _items =
            (await _searchService.SearchItems(
                ProductName,
                ProvinceSelected,
                CantonSelected,
                MinPrice,
                MaxPrice,
                CategorySelected,
                ModelSelected,
                BrandSelected)
            ).ToList();

        // get amount of items found    
        ItemsAmount = _items.Count;

        // order items by sort order
        OrderItems();

        // create paginated list and set it to be displayed
        DisplayItems = PaginatedList<Item>.Create(_items, pageIndex ?? 1, _pageSize);
    }

    /// <summary>
    /// Validates the input received from the user.
    /// </summary>
    /// <param name="province">The selected province for search.</param>
    /// <param name="canton">The selected canton for search.</param>
    /// <param name="minValue">The minimum price filter.</param>
    /// <param name="maxValue">The maximum price filter.</param>
    /// <param name="category">The selected category for search.</param>
    /// <param name="model">The selected model for search.</param>
    /// <param name="brand">The selected brand for search.</param>
    private void ValidateInput(
        string province,
        string canton,
        long minValue,
        long maxValue,
        string category,
        string model,
        string brand)
    {
        if (!string.IsNullOrEmpty(province) && province.Equals("Ninguno")) province = null;

        if (!string.IsNullOrEmpty(canton) && canton.Equals("Ninguno")) canton = null;

        if (!string.IsNullOrEmpty(category) && category.Equals("Ninguno")) category = null;

        ProvinceSelected = province;
        CantonSelected = canton;
        MinPrice = minValue;
        MaxPrice = maxValue;
        CategorySelected = category;
        ModelSelected = model;
        BrandSelected = brand;
    }

    /// <summary>
    /// Sets the sorting parameters based on the user's selection.
    /// </summary>
    /// <param name="sortOrder">The order in which to sort the results.</param>
    /// <param name="sorting">Indicates whether sorting is applied.</param>
    private void SetSortingParameters(string sortOrder, bool sorting)
    {
        if (!sorting)
        {
            if (!string.IsNullOrEmpty(sortOrder)) NameSort = sortOrder;
            return;
        }

        if (string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name_asc"))
        {
            NameSort = "name_desc";
            ProvinceSort = "province_asc";
            CantonSort = "canton_asc";
        }
        else if (sortOrder.Equals("name_desc"))
        {
            NameSort = "name_asc";
        }
        else if (sortOrder.Equals("province_asc"))
        {
            ProvinceSort = "province_desc";
        }
        else if (sortOrder.Equals("province_desc"))
        {
            ProvinceSort = "province_asc";
        }
        else if (sortOrder.Equals("canton_asc"))
        {
            CantonSort = "canton_desc";
        }
        else if (sortOrder.Equals("canton_desc"))
        {
            CantonSort = "canton_asc";
        }
    }


    /// <summary>
    /// Orders the items based on the selected sorting parameters.
    /// </summary>
    private void OrderItems()
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

        if (!string.IsNullOrEmpty(ProvinceSort))
            switch (ProvinceSort)
            {
                case "province_desc":
                    _items = _items.OrderByDescending(item => item.Province).ToList();
                    break;
                case "province_asc":
                    _items = _items.OrderBy(item => item.Province).ToList();
                    break;
            }

        if (!string.IsNullOrEmpty(CantonSort))
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


    /// <summary>
    /// Retrieves the view component for the advanced search modal.
    /// </summary>
    /// <returns>The view component for the advanced search modal.</returns>
    public IActionResult OnGetAdvancedSearch()
    {
        // generate the view component
        var viewComponentResult = ViewComponent("AdvancedSearch", _advancedSearchServiceHandler);

        // return it for it to be integrated
        return viewComponentResult;
    }

    /// <summary>
    /// Updates the cantons and province selected for the advanced search modal.
    /// </summary>
    /// <param name="province">The selected province for updating cantons.</param>
    /// <returns>The updated list of cantons for the selected province.</returns>
    public async Task<IActionResult> OnGetUpdateProvince(string province)
    {
        var cantonsJson = "";


        if (province.Equals("Ninguno"))
        {
            // create empty list
            var emptyCantonList = new List<Canton>
            {
                // add none back as an option
                new()
                {
                    CountryName = "Ninguno",
                    Name = "Ninguno",
                    ProvinceName = "Ninguno"
                }
            };

            // set new list to service canton list
            _advancedSearchServiceHandler.Cantons = emptyCantonList;
        }
        else
        {
            // update the model with all cantons in the given province
            await _advancedSearchServiceHandler.ObtainCantonsAsync(province);
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