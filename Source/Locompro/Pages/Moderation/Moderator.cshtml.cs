using System.Security.Authentication;
using Locompro.Common;
using Locompro.Common.Mappers;
using Locompro.Common.Search;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Models.Dtos;
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
        IAuthService authService,IUserService userService,
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

        await PopulatePageData(pageIndex, 1);
        var testAutoReport = new AutoReportVm()
        {
            Product = "test",
            Store = "paoao",
            Description = "test",
            Confidence = 0.5f,
            Price = 100,
            MinimumPrice = 50,
            MaximumPrice = 150,
            AveragePrice = 100
        };
        var testAutoReport2 = new AutoReportVm()
        {
            Product = "test2",
            Store = "paoao2",
            Description = "test2",
            Confidence = 0.5f,
            Price = 100,
            MinimumPrice = 50,
            MaximumPrice = 150,
            AveragePrice = 100
        };

        AutoReportDisplayItems = PaginatedList<AutoReportVm>.Create(
            new List<AutoReportVm>()
            {
                testAutoReport,
                testAutoReport2
            },
            0,
            _configuration.GetValue("PageSize", 4));
        try
        {
            MostReportedUsers = _userService.GetMostReportedUsersInfo();
        }
        catch (Exception e)
        {
            Logger.LogError("Error obtaining most reported Users in Moderator", e);
            MostReportedUsers = new List<MostReportedUsersResult>();
        }
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
    private async Task PopulatePageData(int? pageIndex, int minAmountOfReports)
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