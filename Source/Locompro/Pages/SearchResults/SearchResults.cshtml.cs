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
using Locompro.Common.Search;
using Locompro.Common.Search.Interfaces;
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Pages.Shared;
using Locompro.Pages.Util;
using Microsoft.Extensions.Configuration;
using Locompro.Services.Domain;
using System;
using System.IO;
using System.Text;

namespace Locompro.Pages.SearchResults;

/// <summary>
/// Page model for the search results page
/// </summary>
public class SearchResultsModel : SearchPageModel
{
    private readonly ISearchService _searchService;

    private readonly IPicturesService _picturesService;

    /// <summary>
    /// Buffer for page size according to Paginated List and configuration
    /// </summary>
    private readonly int _pageSize;

    /// <summary>
    /// List of all items found
    /// </summary>
    private List<Item> _items;

    public SearchViewModel SearchViewModel { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="searchService"></param>
    /// <param name="advancedSearchServiceHandler"></param>
    /// <param name="picturesService"></param>
    /// <param name="configuration"></param>
    /// <param name="httpContextAccessor"></param>
    public SearchResultsModel(
        AdvancedSearchInputService advancedSearchServiceHandler,
        IPicturesService picturesService,
        IConfiguration configuration,
        ISearchService searchService,
        IHttpContextAccessor httpContextAccessor)
        : base(advancedSearchServiceHandler, httpContextAccessor)
    {
        _searchService = searchService;
        _picturesService = picturesService;
        _pageSize = configuration.GetValue("PageSize", 4);
        SearchViewModel = new SearchViewModel
        {
            ResultsPerPage = _pageSize
        };
    }

    /// <summary>
    /// Gets the items to be displayed in the search results
    /// </summary>
    /// <param name="query"></param>
    /// <param name="province"></param>
    /// <param name="canton"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="category"></param>
    /// <param name="model"></param>
    /// <param name="brand"></param>
    public void OnGetAsync(
        string query,
        string province,
        string canton,
        long minValue,
        long maxValue,
        string category,
        string model,
        string brand)
    {
        // validate input
        ValidateInput(province, canton, minValue, maxValue, category, model, brand);
        
        SearchViewModel.ProductName = query;
        
        CacheDataInSession(SearchViewModel, "SearchData");
    }

    public async Task<IActionResult> OnGetGetSearchResultsAsync()
    {
        SearchViewModel = GetCachedDataFromSession<SearchViewModel>("SearchData");
        
        // get items from search service
        List<ISearchCriterion> searchParameters = new List<ISearchCriterion>()
        {
            new SearchCriterion<string>(SearchParameterTypes.Name, SearchViewModel.ProductName),
            new SearchCriterion<string>(SearchParameterTypes.Province, SearchViewModel.ProvinceSelected),
            new SearchCriterion<string>(SearchParameterTypes.Canton, SearchViewModel.CantonSelected),
            new SearchCriterion<long>(SearchParameterTypes.Minvalue, SearchViewModel.MinPrice),
            new SearchCriterion<long>(SearchParameterTypes.Maxvalue, SearchViewModel.MaxPrice),
            new SearchCriterion<string>(SearchParameterTypes.Category, SearchViewModel.CategorySelected),
            new SearchCriterion<string>(SearchParameterTypes.Model, SearchViewModel.ModelSelected),
            new SearchCriterion<string>(SearchParameterTypes.Brand, SearchViewModel.BrandSelected),
        };

        _items = await this._searchService.GetSearchResults(searchParameters);
        
        string searchResultsJson = GetJsonFrom(
            new{
                searchResults = _items,
                data = SearchViewModel}
            );

        return Content(searchResultsJson);
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
        if (!string.IsNullOrEmpty(province) && province.Equals(SearchPageModel.EmptyValue))
        {
            province = null;
        }
        
        if (!string.IsNullOrEmpty(canton) && canton.Equals(SearchPageModel.EmptyValue))
        {
            canton = null;
        }

        if (!string.IsNullOrEmpty(category) && category.Equals(SearchPageModel.EmptyValue))
        {
            category = null;
        } 
        
        SearchViewModel.ProvinceSelected = province;
        SearchViewModel.CantonSelected = canton;
        SearchViewModel.MinPrice = minValue;
        SearchViewModel.MaxPrice = maxValue;
        SearchViewModel.CategorySelected = category;
        SearchViewModel.ModelSelected = model;
        SearchViewModel.BrandSelected = brand;
    }
    
    public async Task<ContentResult> OnGetGetPicturesAsync(string productName, string storeName)
    {
        List<Picture> itemPictures = await _picturesService.GetPicturesForItem(5, productName, storeName);

        List<string> formattedPictures = PictureParser.Serialize(itemPictures);
        
        if (itemPictures.IsNullOrEmpty())
        {
            const string defaultPictureFilePath = "wwwroot/Pictures/No_Image_Picture.png";
            
            byte[] defaultPicture = await System.IO.File.ReadAllBytesAsync(defaultPictureFilePath);
            
            formattedPictures.Add(PictureParser.SerializeData(defaultPicture));
        }
       
        // return list of pictures serialized as json
        return Content(JsonConvert.SerializeObject(formattedPictures));
    }
}