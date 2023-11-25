using System.Drawing.Printing;
using Locompro.Common;
using Locompro.Common.Mappers;
using Locompro.Common.Search;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Common.Search.SearchQueryParameters;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
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
    public long ItemsAmount { get; set; }
    
    public PaginatedList<ModerationSubmissionVm> DisplayItems { get; set; }
    
    public List<ModerationSubmissionVm> Items { get; set; }

    private readonly ISearchService _searchService;
    
    private readonly IModerationService _moderationService;
    
    private readonly IConfiguration _configuration;
    
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
    
    /// <summary>
    /// Creates html for the page on get request
    /// </summary>
    /// <param name="pageIndex"></param>
    public async Task OnGet(int? pageIndex)
    {
        await PopulatePageData(pageIndex, 1);
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
        } catch (Exception e)
        {
            Logger.LogError(e, "Error while acting on report");
        }
        
        Items = GetCachedDataFromSession<List<ModerationSubmissionVm>>("StoredData", false);
        
        ModerationSubmissionVm submissionVmToRemove = Items.Find(moderationSubmissionVm =>
            moderationSubmissionVm.UserId == moderatorActionOnReportVm.SubmissionUserId &&
            moderationSubmissionVm.EntryTime.Year == moderatorActionOnReportVm.SubmissionEntryTime.Year &&
            moderationSubmissionVm.EntryTime.Month == moderatorActionOnReportVm.SubmissionEntryTime.Month &&
            moderationSubmissionVm.EntryTime.Day == moderatorActionOnReportVm.SubmissionEntryTime.Day &&
            moderationSubmissionVm.EntryTime.Hour == moderatorActionOnReportVm.SubmissionEntryTime.Hour &&
            moderationSubmissionVm.EntryTime.Minute == moderatorActionOnReportVm.SubmissionEntryTime.Minute &&
            moderationSubmissionVm.EntryTime.Second == moderatorActionOnReportVm.SubmissionEntryTime.Second);

        Items.Remove(submissionVmToRemove);
             
        CacheDataInSession(Items, "StoredDataBetweenLoads");
        
        DisplayItems =
            PaginatedList<ModerationSubmissionVm>.Create(
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
        Items = GetCachedDataFromSession<List<ModerationSubmissionVm>>("StoredDataBetweenLoads", false);
        
        if (Items == null)
        {
            await GetDataFromDataBase(minAmountOfReports);
        }
        
        ItemsAmount = Items.Count;
        
        DisplayItems =
            PaginatedList<ModerationSubmissionVm>.Create(
                Items, 
                pageIndex?? 0,
                _configuration.GetValue("PageSize", 4));
        
        CacheDataInSession(Items, "StoredData");
    }
    
    private async Task GetDataFromDataBase(int minAmountOfReports)
    {
        ISearchQueryParameters<Submission> searchCriteria = new SearchQueryParameters<Submission>();
        searchCriteria
            .AddQueryParameter(SearchParameterTypes.SubmissionByNAmountReports, minAmountOfReports);

        SubmissionsDto submissionsDto = null;
        
        try
        {
            submissionsDto = await _searchService.GetSearchSubmissions(searchCriteria);
        } catch (Exception e)
        {
            Logger.LogError(e, "Error while getting search results");
        }
        
        ModerationSubmissionsMapper mapper = new ();
        Items = mapper.ToVm(submissionsDto);
    }
}