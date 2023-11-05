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
using Locompro.Models;
using Locompro.Models.ViewModels;
using Locompro.Pages.Shared;
using Locompro.Pages.Util;
using Microsoft.Extensions.Configuration;
using Locompro.Services.Domain;
using System;
using System.IO;
using System.Text;
using Locompro.Common.Search.SearchMethodRegistration;

namespace Locompro.Pages.SearchResults;

/// <summary>
/// Page model for the search results page
/// </summary>
public class SearchResultsModel : SearchPageModel
{
    private IConfiguration Configuration { get; set; }
    public SearchViewModel SearchViewModel { get; set; }
    
    private readonly ISearchService _searchService;

    private readonly IPictureService _pictureService;
    
    private readonly ISubmissionService _submissionService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="searchService"></param>
    /// <param name="advancedSearchServiceHandler"></param>
    /// <param name="pictureService"></param>
    /// <param name="configuration"></param>
    /// <param name="submissionService"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="httpContextAccessor"></param>
    public SearchResultsModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        AdvancedSearchInputService advancedSearchServiceHandler,
        IPictureService pictureService,
        IConfiguration configuration,
        ISearchService searchService,
        ISubmissionService submissionService)
        : base(loggerFactory, httpContextAccessor, advancedSearchServiceHandler)
    {
        _searchService = searchService;
        _pictureService = pictureService;
        Configuration = configuration;
        SearchViewModel = new SearchViewModel
        {
            ResultsPerPage = Configuration.GetValue("PageSize", 4)
        };
        
        _searchService = searchService;
        _pictureService = pictureService;
        _submissionService = submissionService;
    }

    /// <summary>
    /// When page is first called, gets search query data from session
    /// sent by either another search or search from another source
    /// Since html is returned, it is not possible to send a model
    /// so instead it stores the search data and waits for page to request it after building html 
    /// </summary>
    public void OnGetAsync()
    {
        // prevents system from crashing, but in essence, leads to a re-request where data is no longer null
        SearchViewModel = GetCachedDataFromSession<SearchViewModel>("SearchQueryViewModel", false) ?? new SearchViewModel();

        ValidateInput();
        
        CacheDataInSession(SearchViewModel, "SearchData");
    }
    
    /// <summary>
    /// When requesting search results, fetches search query data and returns search results
    /// </summary>
    /// <returns> json file with search results and search info data</returns>
    public async Task<IActionResult> OnGetGetSearchResultsAsync()
    {
        SearchViewModel = GetCachedDataFromSession<SearchViewModel>("SearchData", false);
        
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
        
        SearchViewModel.ResultsPerPage = Configuration.GetValue("PageSize", 4);

        List<Item> searchResults = await _searchService.GetSearchResults(searchParameters);
        
        string searchResultsJson = GetJsonFrom(
            new{
                SearchResults = searchResults,
                Data = SearchViewModel
            });

        return Content(searchResultsJson);
    }
    
    /// <summary>
    /// Returns a list of pictures for a given item
    /// </summary>
    /// <param name="productName"> product name of an item </param>
    /// <param name="storeName"> store name of an item</param>
    /// <returns></returns>
    public async Task<ContentResult> OnGetGetPicturesAsync(string productName, string storeName)
    {
        List<Picture> itemPictures = await _pictureService.GetPicturesForItem(5, productName, storeName);

        List<string> formattedPictures = PictureParser.Serialize(itemPictures);
        
        if (itemPictures.IsNullOrEmpty())
        {
            const string defaultPictureFilePath = "wwwroot/Pictures/No_Image_Picture.png";
            
            byte[] defaultPicture = await System.IO.File.ReadAllBytesAsync(defaultPictureFilePath);
            
            formattedPictures.Add(PictureParser.SerializeData(defaultPicture));
        }
       
        // return list of pictures serialized as json
        return Content(GetJsonFrom(formattedPictures));
    }
    
    /// <summary>
    /// Validates if the input provided by the user is valid
    /// </summary>
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
    /// Updates the rating of a given submission
    /// </summary>
    public async Task OnPostUpdateSubmissionRatingAsync()
    {
        RatingViewModel clientRatingChange = await GetDataSentByClient<RatingViewModel>();
        
        if (clientRatingChange != null)
        {
            await _submissionService.UpdateSubmissionRating(
                clientRatingChange.SubmissionUserId,
                clientRatingChange.SubmissionEntryTime,
                int.Parse(clientRatingChange.Rating));
        }
    }
}