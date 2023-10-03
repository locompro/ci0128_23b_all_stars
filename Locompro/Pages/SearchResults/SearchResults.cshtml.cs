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
/// Page model for the search results page
/// </summary>
public class SearchResultsModel : PageModel
{
    /// <summary>
    /// Service that handles the advanced search modal
    /// </summary>
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

    public string CurrentFilter { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="searchService"></param>
    /// <param name="advancedSearchServiceHandler"></param>
    /// <param name="configuration"></param>
    public SearchResultsModel(
        IConfiguration configuration,
        SearchService searchService)
    {
        this._searchService = searchService;
        this._configuration = configuration;
        this._pageSize = _configuration.GetValue("PageSize", 4);
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
        string sortOrder)
    {
        this.ProductName = query;

        // set up sorting parameters
        this.SetSortingParameters(sortOrder, (sorting is not null));

        // get items from search service
        this._items =
            (await _searchService.SearchItems(
                this.ProductName,
                this.ProvinceSelected,
                this.CantonSelected,
                this.MinPrice,
                this.MaxPrice,
                this.CategorySelected,
                this.ModelSelected,
                this.BrandSelected)
            ).ToList();

        // get amount of items found    
        this.ItemsAmount = _items.Count;

        // order items by sort order
        this.OrderItems();

        // create paginated list and set it to be displayed
        this.DisplayItems = PaginatedList<Item>.Create(_items, pageIndex ?? 1, _pageSize);
    }

    /// <summary>
    /// Manages all sorting done to items in list
    /// </summary>
    /// <param name="sortOrder"></param>
    /// <param name="sorting"></param>
    private void SetSortingParameters(string sortOrder, bool sorting)
    {
        if (!sorting)
        {
            if (!string.IsNullOrEmpty(sortOrder))
            {
                this.NameSort = sortOrder;
            }
            return;
        }

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
    void OrderItems()
    {
        switch (this.NameSort)
        {
            case "name_desc":
                _items = _items.OrderByDescending(item => item.ProductName).ToList();
                break;
            case "name_asc":
                _items = _items.OrderBy(item => item.ProductName).ToList();
                break;
        }
    }
}