using System.Net;
using System.Runtime.CompilerServices;
using Castle.Core.Internal;
using Locompro.Common;
using Locompro.Common.Mappers;
using Locompro.Common.Search;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Common.Search.SearchQueryParameters;
using Locompro.Models;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Pages.Shared;
using Locompro.Pages.Util;
using Locompro.Services;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace Locompro.Pages.SearchResults;

/// <summary>
///     Page model for the search results page
/// </summary>
public class SearchResultsModel : SearchPageModel
{
    public SearchVm SearchVm { get; set; }

    private readonly IAuthService _authService;

    private readonly IModerationService _moderationService;

    private readonly IPictureService _pictureService;

    private readonly ISearchService _searchService;

    private readonly ISubmissionService _submissionService;

    private IConfiguration Configuration { get; set; }

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="advancedSearchServiceHandler"></param>
    /// <param name="pictureService"></param>
    /// <param name="configuration"></param>
    /// <param name="searchService"></param>
    /// <param name="submissionService"></param>
    /// <param name="moderationService"></param>
    /// <param name="authService"></param>
    /// <param name="apiKeyHandler"></param>
    public SearchResultsModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        AdvancedSearchInputService advancedSearchServiceHandler,
        IPictureService pictureService,
        IConfiguration configuration,
        ISearchService searchService,
        ISubmissionService submissionService,
        IModerationService moderationService,
        IAuthService authService,
        IApiKeyHandler apiKeyHandler)
        : base(loggerFactory, httpContextAccessor, advancedSearchServiceHandler, apiKeyHandler)
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
        _moderationService = moderationService;
        _authService = authService;
    }

    /// <summary>
    ///     When page is first called, gets search query data from session
    ///     sent by either another search or search from another source
    ///     Since html is returned, it is not possible to send a model
    ///     so instead it stores the search data and waits for page to request it after building html
    /// </summary>
    public void OnGetAsync()
    {
        // prevents system from crashing, but in essence, leads to a re-request where data is no longer null
        SearchVm = GetCachedDataFromSession<SearchVm>("SearchQueryViewModel", false) ?? new SearchVm();

        ValidateInput();

        CacheDataInSession(SearchVm, "SearchData");
    }

    /// <summary>
    ///     When requesting search results, fetches search query data and returns search results
    /// </summary>
    /// <returns> json file with search results and search info data</returns>
    public async Task<IActionResult> OnGetGetSearchResultsAsync()
    {
        SearchVm = GetCachedDataFromSession<SearchVm>("SearchData", false);
        SearchVm.ResultsPerPage = Configuration.GetValue("PageSize", 4);

        List<ItemVm> searchResults = null;

        try
        {
            ItemMapper itemMapper = new();
            SubmissionsDto submissionsDto = await _searchService.GetSearchResultsAsync(SearchVm);
            searchResults = itemMapper.ToVm(submissionsDto);
        }
        catch (Exception e)
        {
            Logger.LogError("Error when attempting to get search results: " + e.Message);
        }

        var searchResultsJson = GetJsonFrom(
            new
            {
                SearchResults = searchResults,
                Data = SearchVm,
                Redirect = SearchVm.IsEmpty() ? "redirect" : null
            });

        return Content(searchResultsJson);
    }

    public async Task<IActionResult> OnGetGetUsersReportedSubmissions()
    {
        if (!_authService.IsLoggedIn())
        {
            Response.StatusCode = 302; // Redirect status code
            return new JsonResult(Array.Empty<object>());
        }

        try
        {
            var userId = _authService.GetUserId();

            var reportedSubmissions = await _moderationService.GetUsersReportedSubmissions(userId);

            var reportedSubmissionVms = 
                reportedSubmissions.Select(rs => new SubmissionVm(rs, GetFormattedDate));

            return Content(GetJsonFrom(reportedSubmissionVms));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to get user's reported submissions");

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    ///     Returns a list of pictures for a given item
    /// </summary>
    /// <param name="productId"> id name of an item</param>
    /// <param name="storeName"> store name of an item</param>
    /// <returns></returns>
    public async Task<ContentResult> OnGetGetPicturesAsync(int productId, string storeName)
    {
        List<PictureDto> pictureDtos = null;

        try
        {
            pictureDtos = await _pictureService.GetPicturesForItem(10, productId, storeName);
        }
        catch (Exception e)
        {
            Logger.LogError("Error when attempting to get pictures for item: " + e.Message);
        }

        List<string> formattedPictures = new();

        if (pictureDtos == null || pictureDtos.IsNullOrEmpty())
        {
            const string defaultPictureFilePath = "wwwroot/Pictures/No_Image_Picture.png";

            var defaultPicture = await System.IO.File.ReadAllBytesAsync(defaultPictureFilePath);

            formattedPictures.Add(PictureParser.SerializeData(defaultPicture));
        }
        else
        {
            IMapper<PictureDto, PictureVm> pictureMapper = new PictureMapper();
            
            List<PictureVm> pictureVms = new();
            
            pictureVms.AddRange(pictureDtos.Select(pictureDto => pictureMapper.ToVm(pictureDto)));

            formattedPictures = PictureParser.Serialize(pictureVms);
        }

        // return list of pictures serialized as json
        return Content(GetJsonFrom(formattedPictures));
    }


    public async Task<JsonResult> OnPostReportSubmissionAsync(UserReportVm userReportVm)
    {
        if (!_authService.IsLoggedIn())
        {
            Response.StatusCode = 302; // Redirect status code
            return new JsonResult(new { redirectUrl = "/Account/Login" });
        }

        try
        {
            var reportMapper = new ReportMapper();

            var reportDto = reportMapper.ToDto(userReportVm);

            reportDto.UserId = _authService.GetUserId();

            await _moderationService.ReportSubmission(reportDto);

            Logger.LogInformation("Report submitted successfully {}", userReportVm);

            return new JsonResult(new { success = true, message = "Report submitted successfully" });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to submit report {}", userReportVm);

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    ///     Validates if the input provided by the user is valid
    /// </summary>
    private void ValidateInput()
    {
        if (!string.IsNullOrEmpty(SearchVm.ProvinceSelected) && SearchVm.ProvinceSelected.Equals(EmptyValue))
            SearchVm.ProvinceSelected = null;

        if (!string.IsNullOrEmpty(SearchVm.CantonSelected) && SearchVm.CantonSelected.Equals(EmptyValue))
            SearchVm.CantonSelected = null;

        if (!string.IsNullOrEmpty(SearchVm.CategorySelected) && SearchVm.CategorySelected.Equals(EmptyValue))
            SearchVm.CategorySelected = null;
    }

    /// <summary>
    ///     Updates the rating of a given submission
    /// </summary>
    public async Task<JsonResult> OnPostUpdateSubmissionRatingAsync()
    {
        if (!_authService.IsLoggedIn())
        {
            Response.StatusCode = 302; // Redirect status code
            return new JsonResult(new { redirectUrl = "/Account/Login" });
        }

        var clientRatingChange = await GetDataSentByClient<RatingVm>();

        if (clientRatingChange == null)
            Logger.LogError("Client rating change was null when attempting to update submission rating");

        try
        {
            await _submissionService.UpdateSubmissionRating(clientRatingChange);
        }
        catch (Exception e)
        {
            Logger.LogError("Error when attempting to update submission rating: " + e.Message);
        }

        return new JsonResult(new { ok = true, message = "Ratings updated submitted successfully" });
    }

    /// <summary>
    ///     Extracts from entry time, the date in the format yyyy-mm-dd
    ///     to be shown in the results page
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    private static string GetFormattedDate(Submission submission)
    {
        return DateFormatter.GetFormattedDateFromDateTime(submission.EntryTime);
    }
}