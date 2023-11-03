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

    public SearchViewModel SearchViewModel { get; set; }
    
    private IConfiguration _configuration { get; set; }

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
        _configuration = configuration;
        SearchViewModel = new SearchViewModel
        {
            ResultsPerPage = _configuration.GetValue("PageSize", 4)
        };
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnGetAsync()
    {
        // prevents system from crashing, but in essence, leads to a re-request where data is no longer null
        SearchViewModel = GetCachedDataFromSession<SearchViewModel>("SearchQueryViewModel", false) ?? new SearchViewModel();

        ValidateInput();
        
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
        
        SearchViewModel.ResultsPerPage = _configuration.GetValue("PageSize", 4);

        List<Item> searchResults = await _searchService.GetSearchResults(searchParameters);
        
        string searchResultsJson = GetJsonFrom(
            new{
                SearchResults = searchResults,
                Data = SearchViewModel }
            );

        return Content(searchResultsJson);
    }
    
    private void ValidateInput()
    {
        if (!string.IsNullOrEmpty(SearchViewModel.ProvinceSelected) && SearchViewModel.ProvinceSelected.Equals(SearchPageModel.EmptyValue))
        {
            SearchViewModel.ProvinceSelected = null;
        }
        
        if (!string.IsNullOrEmpty(SearchViewModel.CantonSelected) && SearchViewModel.CantonSelected.Equals(SearchPageModel.EmptyValue))
        {
            SearchViewModel.CantonSelected = null;
        }

        if (!string.IsNullOrEmpty(SearchViewModel.CategorySelected) && SearchViewModel.CategorySelected.Equals(SearchPageModel.EmptyValue))
        {
            SearchViewModel.CategorySelected = null;
        } 
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productName"></param>
    /// <param name="storeName"></param>
    /// <returns></returns>
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