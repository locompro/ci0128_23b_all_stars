using Locompro.Common;
using Locompro.Common.Mappers;
using Locompro.Common.Search;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;
using Locompro.Pages.Shared;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Locompro.Pages.Moderation;

/// <summary>
/// Moderator page model
/// Displays all submissions that have been reported
/// </summary>
public class ModeratorPageModel : BasePageModel
{
    private readonly IConfiguration _configuration;

    private readonly IModerationService _moderationService;

    private readonly ISearchService _searchService;

    public ModeratorPageModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        ISearchService service,
        IModerationService moderationService,
        IConfiguration configuration) : base(loggerFactory, httpContextAccessor)
    {
        _searchService = service;
        _configuration = configuration;
        _moderationService = moderationService;
    }

    public long ItemsAmount { get; set; }

    public PaginatedList<UserReportedSubmissionVm> UserReportDisplayItems { get; set; }

    public PaginatedList<AutoReportVm> AutoReportDisplayItems { get; set; }

    public List<UserReportedSubmissionVm> Items { get; set; }

    /// <summary>
    /// Creates html for the page on get request
    /// </summary>
    /// <param name="pageIndex"></param>
    public async Task OnGet(int? pageIndex)
    {
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
    }

    /// <summary>
    /// On post receives the moderator action on a report
    /// </summary>
    public async Task<PageResult> OnPostActOnReport()
    {
        ModeratorActionOnReportVm moderatorActionOnReportVm = await GetDataSentByClient<ModeratorActionOnReportVm>();

        try
        {
            await _moderationService.ActOnReport(moderatorActionOnReportVm);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while acting on report");
        }

        Items = GetCachedDataFromSession<List<UserReportedSubmissionVm>>("StoredData", false);

        UserReportedSubmissionVm submissionVmToRemove = Items.Find(userReportedSubmissionVm =>
            userReportedSubmissionVm.UserId == moderatorActionOnReportVm.SubmissionUserId &&
            userReportedSubmissionVm.EntryTime.Year == moderatorActionOnReportVm.SubmissionEntryTime.Year &&
            userReportedSubmissionVm.EntryTime.Month == moderatorActionOnReportVm.SubmissionEntryTime.Month &&
            userReportedSubmissionVm.EntryTime.Day == moderatorActionOnReportVm.SubmissionEntryTime.Day &&
            userReportedSubmissionVm.EntryTime.Hour == moderatorActionOnReportVm.SubmissionEntryTime.Hour &&
            userReportedSubmissionVm.EntryTime.Minute == moderatorActionOnReportVm.SubmissionEntryTime.Minute &&
            userReportedSubmissionVm.EntryTime.Second == moderatorActionOnReportVm.SubmissionEntryTime.Second);

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
        List<ISearchCriterion> searchCriteria = new List<ISearchCriterion>()
        {
            new SearchCriterion<int>(SearchParameterTypes.HasNAmountReports, minAmountOfReports)
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