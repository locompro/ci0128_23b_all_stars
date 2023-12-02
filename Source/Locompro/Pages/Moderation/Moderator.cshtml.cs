using System.Security.Authentication;
using Locompro.Common;
using Locompro.Common.Mappers;
using Locompro.Common.Search;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Common.Search.SearchQueryParameters;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.Results;
using Locompro.Models.ViewModels;
using Locompro.Pages.Shared;
using Locompro.Services;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Moderation;

/// <summary>
/// Moderator page model
/// Displays all submissions that have been reported
/// </summary>
public class ModeratorPageModel : BasePageModel
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    private readonly IModerationService _moderationService;

    private readonly ISearchService _searchService;
    private readonly IUserService _userService;

    public ModeratorPageModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        ISearchService service,
        IModerationService moderationService,
        IAuthService authService, IUserService userService,
        IConfiguration configuration) : base(loggerFactory, httpContextAccessor)
    {
        _searchService = service;
        _configuration = configuration;
        _moderationService = moderationService;
        _authService = authService;
        _userService = userService;
    }

    public PaginatedList<UserReportedSubmissionVm> UserReportDisplayItems { get; set; }

    public List<MostReportedUsersResult> MostReportedUsers { get; set; }

    public PaginatedList<AutoReportVm> AutoReportDisplayItems { get; set; }

    public List<UserReportedSubmissionVm> UserReportedSubmissionItems { get; set; } = new();

    public List<AutoReportVm> AutoReportItems { get; set; } = new();

    /// <summary>
    /// Creates html for the page on get request
    /// </summary>
    /// <param name="userReportPageIndex"></param>
    /// <param name="autoReportPageIndex"></param>
    public async Task OnGet(int? userReportPageIndex, int? autoReportPageIndex)
    {
        if (!_authService.IsLoggedIn())
        {
            throw new AuthenticationException("No user is logged in");
        }

        await BuildPageContents(userReportPageIndex ?? 0);
    }

    /// <summary>
    /// On post receives the moderator action on a report
    /// </summary>
    public async Task<PageResult> OnPostActOnReport()
    {
        var moderatorActionVm = await GetDataSentByClient<ModeratorActionVm>();

        var moderatorActionMapper = new ModeratorActionMapper();

        var moderatorActionDto = moderatorActionMapper.ToDto(moderatorActionVm);

        moderatorActionDto.ModeratorId = _authService.GetUserId();
        try
        {
            await _moderationService.ActOnReport(moderatorActionDto);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while acting on report");

            await BuildPageContents();
            return Page();
        }

        //UserReportedSubmissionItems = GetCachedDataFromSession<List<UserReportedSubmissionVm>>("StoredData", false);

        /*UserReportedSubmissionVm submissionVmToRemove = UserReportedSubmissionItems.Find(userReportedSubmissionVm =>
            userReportedSubmissionVm.UserId == moderatorActionVm.SubmissionUserId &&
            userReportedSubmissionVm.EntryTime.Year == moderatorActionVm.SubmissionEntryTime.Year &&
            userReportedSubmissionVm.EntryTime.Month == moderatorActionVm.SubmissionEntryTime.Month &&
            userReportedSubmissionVm.EntryTime.Day == moderatorActionVm.SubmissionEntryTime.Day &&
            userReportedSubmissionVm.EntryTime.Hour == moderatorActionVm.SubmissionEntryTime.Hour &&
            userReportedSubmissionVm.EntryTime.Minute == moderatorActionVm.SubmissionEntryTime.Minute &&
            userReportedSubmissionVm.EntryTime.Second == moderatorActionVm.SubmissionEntryTime.Second);

        UserReportedSubmissionItems.Remove(submissionVmToRemove);*/

        /*CacheDataInSession(UserReportedSubmissionItems, "StoredDataBetweenLoads");*/

        await BuildPageContents();
        return Page();
    }

    private async Task BuildPageContents(int pageIndex = 0, int minReports = 1)
    {
        await PopulateUserReportData(pageIndex, minReports);
        await PopulateAutoReportData(pageIndex);
        PopulateMostReportedModal();
    }

    /// <summary>
    /// Fills internal class data with the data to be displayed on the page
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="minAmountOfReports"></param>
    private async Task PopulateUserReportData(int? pageIndex = 0, int minAmountOfReports = 1)
    {
        UserReportedSubmissionItems =
            GetCachedDataFromSession<List<UserReportedSubmissionVm>>("StoredDataBetweenLoads", false);

        if (UserReportedSubmissionItems == null)
        {
            await GetUserReports(minAmountOfReports);
        }

        UserReportDisplayItems =
            PaginatedList<UserReportedSubmissionVm>.Create(
                UserReportedSubmissionItems,
                pageIndex ?? 0,
                _configuration.GetValue("PageSize", 4));

        CacheDataInSession(UserReportedSubmissionItems, "StoredData");
    }

    /// <summary>
    /// Populates the AutoReportDisplayItems with data retrieved from fetchAutoReports.
    /// </summary>
    /// <param name="pageIndex">Optional page index for pagination.</param>
    private async Task PopulateAutoReportData(int? pageIndex = 0)
    {
        List<AutoReportVm> autoReports;
        try
        {
            autoReports = await FetchAutoReports();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while fetching auto reports");

            autoReports = new List<AutoReportVm>();
        }

        AutoReportItems = autoReports;

        AutoReportDisplayItems =
            PaginatedList<AutoReportVm>.Create(
                autoReports,
                pageIndex ?? 0,
                _configuration.GetValue("PageSize", 4));
    }

    private void PopulateMostReportedModal()
    {
        try
        {
            MostReportedUsers = _userService.GetMostReportedUsersInfo();
        }
        catch (Exception e)
        {
            Logger.LogError("Error obtaining most reported Users in Moderator. Error " + e.Message);
            MostReportedUsers = new List<MostReportedUsersResult>();
        }
    }

    /// <summary>
    /// Fetches auto reports from the database using the moderation service
    /// </summary>
    /// <returns> List of AutoReportsVm</returns>
    private async Task<List<AutoReportVm>> FetchAutoReports()
    {
        var userId = _authService.GetUserId();

        // Call moderation services for dto's on auto reports
        var submissionsWithAutoReports = await _moderationService.FetchAllSubmissionsWithAutoReport(userId);
        // Map the dtos to vms 
        AutoReportSubmissionsMapper mapper = new();
        var autoReportVms = mapper.ToVm(submissionsWithAutoReports);

        // Return the vms
        return autoReportVms;
    }

    private async Task GetUserReports(int minAmountOfReports = 1)
    {
        var userId = _authService.GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new AuthenticationException("Retrieved user ID is not valid");
        }

        ISearchQueryParameters<Submission> searchQueryParameters = new SearchQueryParameters<Submission>();
        searchQueryParameters
            .AddQueryParameter(SearchParameterTypes.SubmissionByNAmountReports, minAmountOfReports)
            .AddQueryParameter(SearchParameterTypes.SubmissionHasApproverOrRejecter, userId);

        SubmissionsDto submissionsDto = null;

        try
        {
            submissionsDto = await _searchService.GetSearchSubmissionsAsync(searchQueryParameters);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while getting search results");
        }

        UserReportSubmissionsMapper mapper = new();
        UserReportedSubmissionItems = mapper.ToVm(submissionsDto);
    }
}