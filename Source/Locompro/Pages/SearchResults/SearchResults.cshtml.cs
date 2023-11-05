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
using Locompro.Models.Entities;

namespace Locompro.Pages.SearchResults;

/// <summary>
/// Page model for the search results page
/// </summary>
public class SearchResultsModel : SearchPageModel
{
    private IConfiguration Configuration { get; set; }
    public SearchVm SearchVm { get; set; }
    
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
        SearchVm = new SearchVm
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
        SearchVm = GetCachedDataFromSession<SearchVm>("SearchQueryViewModel", false) ?? new SearchVm();

        ValidateInput();
        
        CacheDataInSession(SearchVm, "SearchData");
    }
    
    /// <summary>
    /// When requesting search results, fetches search query data and returns search results
    /// </summary>
    /// <returns> json file with search results and search info data</returns>
    public async Task<IActionResult> OnGetGetSearchResultsAsync()
    {
        SearchVm = GetCachedDataFromSession<SearchVm>("SearchData", false);
        
        // get items from search service
        List<ISearchCriterion> searchParameters = new List<ISearchCriterion>()
        {
            new SearchCriterion<string>(SearchParameterTypes.Name, SearchVm.ProductName),
            new SearchCriterion<string>(SearchParameterTypes.Province, SearchVm.ProvinceSelected),
            new SearchCriterion<string>(SearchParameterTypes.Canton, SearchVm.CantonSelected),
            new SearchCriterion<long>(SearchParameterTypes.Minvalue, SearchVm.MinPrice),
            new SearchCriterion<long>(SearchParameterTypes.Maxvalue, SearchVm.MaxPrice),
            new SearchCriterion<string>(SearchParameterTypes.Category, SearchVm.CategorySelected),
            new SearchCriterion<string>(SearchParameterTypes.Model, SearchVm.ModelSelected),
            new SearchCriterion<string>(SearchParameterTypes.Brand, SearchVm.BrandSelected),
        };
        
        SearchVm.ResultsPerPage = Configuration.GetValue("PageSize", 4);

        List<ItemVm> searchResults = null;

        try
        {
            searchResults = await _searchService.GetSearchResults(searchParameters);
        } catch (Exception e)
        {
            Logger.LogError("Error when attempting to get search results: " + e.Message);
        }
        
        
        string searchResultsJson = GetJsonFrom(
            new{
                SearchResults = searchResults,
                Data = SearchVm
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
        List<Picture> itemPictures = null;
        
        try
        {
            itemPictures = await _pictureService.GetPicturesForItem(5, productName, storeName);
        } catch (Exception e)
        {
            Logger.LogError("Error when attempting to get pictures for item: " + e.Message);
        }
        

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
        if (!string.IsNullOrEmpty(SearchVm.ProvinceSelected) && SearchVm.ProvinceSelected.Equals(SearchPageModel.EmptyValue))
        {
            SearchVm.ProvinceSelected = null;
        }
        
        if (!string.IsNullOrEmpty(SearchVm.CantonSelected) && SearchVm.CantonSelected.Equals(SearchPageModel.EmptyValue))
        {
            SearchVm.CantonSelected = null;
        }

        if (!string.IsNullOrEmpty(SearchVm.CategorySelected) && SearchVm.CategorySelected.Equals(SearchPageModel.EmptyValue))
        {
            SearchVm.CategorySelected = null;
        } 
    }

    /// <summary>
    /// Updates the rating of a given submission
    /// </summary>
    public async Task OnPostUpdateSubmissionRatingAsync()
    {
        RatingVm clientRatingChange = await GetDataSentByClient<RatingVm>();

        if (clientRatingChange == null)
        {
            Logger.LogError("Client rating change was null when attempting to update submission rating");
        }
        
        try
        {
            await _submissionService.UpdateSubmissionRating(clientRatingChange);
        } catch (Exception e)
        {
            Logger.LogError("Error when attempting to update submission rating: " + e.Message);
        }
    }
}