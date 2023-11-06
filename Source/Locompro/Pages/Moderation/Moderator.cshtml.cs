using System.Drawing.Printing;
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
    public long ItemsAmount { get; set; }
    
    public PaginatedList<ModerationSubmissionVm> DisplayItems { get; set; }

    private readonly ISearchService _searchService;
    
    private readonly ISubmissionService _submissionService;
    
    private readonly IConfiguration _configuration;
    
    public ModeratorPageModel(
        ILoggerFactory loggerFactory,
        IHttpContextAccessor httpContextAccessor,
        ISearchService service,
        ISubmissionService submissionService,
        IConfiguration configuration) : base(loggerFactory, httpContextAccessor)
    {
        _searchService = service;
        _configuration = configuration;
        _submissionService = submissionService;
    }
    
    /// <summary>
    /// Creates html for the page on get request
    /// </summary>
    /// <param name="pageIndex"></param>
    public async Task OnGet(int? pageIndex)
    {
        await PopulatePageData(pageIndex);
    }
    
    /// <summary>
    /// On post receives the moderator action on a report
    /// </summary>
    public async Task OnPostActOnReport()
    {
        ModeratorActionOnReportVm moderatorActionOnReportVm = await GetDataSentByClient<ModeratorActionOnReportVm>();

        await _submissionService.ActOnReport(moderatorActionOnReportVm);
        
        await PopulatePageData(0);
    }
    
    /// <summary>
    /// Fills internal class data with the data to be displayed on the page
    /// </summary>
    /// <param name="pageIndex"></param>
    private async Task PopulatePageData(int? pageIndex)
    {
        List<ISearchCriterion> searchCriteria = new List<ISearchCriterion>()
        {
            new SearchCriterion<int>(SearchParameterTypes.HasNAmountReports, 1)
        };

        SubmissionDto submissionDto = await _searchService.GetSearchResults(searchCriteria);
        
        ModerationSubmissionMapper mapper = new ();
        
        List<ModerationSubmissionVm> reports = mapper.ToVm(submissionDto);
        
        ItemsAmount = reports.Count;
        
        DisplayItems =
            PaginatedList<ModerationSubmissionVm>.Create(
                reports, 
                pageIndex?? 0,
                _configuration.GetValue("PageSize", 4));
    }
}