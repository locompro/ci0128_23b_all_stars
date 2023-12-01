using System.Security.Authentication;
using Locompro.Common;
using Locompro.Common.Mappers;
using Locompro.Common.Search;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Models.Dtos;
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

    public ModeratorPageModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        ISearchService service,
        IModerationService moderationService,
        IAuthService authService,
        IConfiguration configuration) : base(loggerFactory, httpContextAccessor)
    {
        _searchService = service;
        _configuration = configuration;
        _moderationService = moderationService;
        _authService = authService;
    }

    public PaginatedList<UserReportedSubmissionVm> UserReportDisplayItems { get; set; }

    public PaginatedList<AutoReportVm> AutoReportDisplayItems { get; set; }

    public List<UserReportedSubmissionVm> Items { get; set; }

    public int ItemsAmount { get; set; }

    /// <summary>
    /// Creates html for the page on get request
    /// </summary>
    /// <param name="pageIndex"></param>
    public async Task OnGet(int? pageIndex)
    {
        if (!_authService.IsLoggedIn())
        {
            throw new AuthenticationException("No user is logged in");
        }

        await PopulateUserReportData(pageIndex, 1);
        await PopulateAutoReportData(pageIndex);
    }

    /// <summary>
    /// On post receives the moderator action on a report
    /// </summary>
    public async Task<PageResult> OnPostActOnReport()
    {
        ModeratorActionVm moderatorActionVm = await GetDataSentByClient<ModeratorActionVm>();

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

            return Page();
        }

        Items = GetCachedDataFromSession<List<UserReportedSubmissionVm>>("StoredData", false);

        UserReportedSubmissionVm submissionVmToRemove = Items.Find(userReportedSubmissionVm =>
            userReportedSubmissionVm.UserId == moderatorActionVm.SubmissionUserId &&
            userReportedSubmissionVm.EntryTime.Year == moderatorActionVm.SubmissionEntryTime.Year &&
            userReportedSubmissionVm.EntryTime.Month == moderatorActionVm.SubmissionEntryTime.Month &&
            userReportedSubmissionVm.EntryTime.Day == moderatorActionVm.SubmissionEntryTime.Day &&
            userReportedSubmissionVm.EntryTime.Hour == moderatorActionVm.SubmissionEntryTime.Hour &&
            userReportedSubmissionVm.EntryTime.Minute == moderatorActionVm.SubmissionEntryTime.Minute &&
            userReportedSubmissionVm.EntryTime.Second == moderatorActionVm.SubmissionEntryTime.Second);

        Items.Remove(submissionVmToRemove);

        CacheDataInSession(Items, "StoredDataBetweenLoads");

        UserReportDisplayItems =
            PaginatedList<UserReportedSubmissionVm>.Create(
                Items,
                0,
                _configuration.GetValue("PageSize", 4));

        return Page();
    }

    /// <summary>
    /// Fills internal class data with the data to be displayed on the page
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="minAmountOfReports"></param>
    private async Task PopulateUserReportData(int? pageIndex, int minAmountOfReports)
    {
        Items = GetCachedDataFromSession<List<UserReportedSubmissionVm>>("StoredDataBetweenLoads", false);

        if (Items == null)
        {
            await GetDataFromDataBase(minAmountOfReports);
        }

        ItemsAmount = Items.Count;

        UserReportDisplayItems =
            PaginatedList<UserReportedSubmissionVm>.Create(
                Items,
                pageIndex ?? 0,
                _configuration.GetValue("PageSize", 4));

        CacheDataInSession(Items, "StoredData");
    }

    /// <summary>
    /// Populates the AutoReportDisplayItems with data retrieved from fetchAutoReports.
    /// </summary>
    /// <param name="pageIndex">Optional page index for pagination.</param>
    private async Task PopulateAutoReportData(int? pageIndex)
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

        AutoReportDisplayItems =
            PaginatedList<AutoReportVm>.Create(
                autoReports,
                pageIndex ?? 0,
                _configuration.GetValue("PageSize", 4));
    }


    /// <summary>
    /// Fetches auto reports from the database using the moderation service
    /// </summary>
    /// <returns> List of AutoReportsVm</returns>
    private async Task<List<AutoReportVm>> FetchAutoReports()
    {
        return new List<AutoReportVm> () ;
    }

    private async Task GetDataFromDataBase(int minAmountOfReports)
    {
        string userId = _authService.GetUserId();

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new AuthenticationException("Retrieved user ID is not valid");
        }

        List<ISearchCriterion> searchCriteria = new List<ISearchCriterion>()
        {
            new SearchCriterion<int>(SearchParameterTypes.SubmissionByNAmountReports, minAmountOfReports),
            new SearchCriterion<string>(SearchParameterTypes.SubmissionHasApproverOrRejecter, userId)
        };

        SubmissionsDto submissionsDto = null;

        try
        {
            submissionsDto = await _searchService.GetSearchResults(searchCriteria);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while getting search results");
        }

        ModerationSubmissionsMapper mapper = new();
        Items = mapper.ToVm(submissionsDto);
    }
}