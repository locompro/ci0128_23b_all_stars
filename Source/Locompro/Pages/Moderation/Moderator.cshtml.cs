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

public class ModeratorPageModel : BasePageModel
{
    public long ItemsAmount { get; set; }
    
    public PaginatedList<ModerationSubmissionVm> DisplayItems { get; set; }
    
    public ModerationSubmissionVm SelectedSubmission { get; set; }

    private readonly ISearchService _searchService;
    
    private readonly ISubmissionService _submissionService;
    
    private readonly IConfiguration _configuration;

    private List<ModerationSubmissionVm> _reports;
    
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
    
    public async Task OnGet(int? pageIndex)
    {
        await PopulatePageData(pageIndex);
    }

    public async Task OnPostActOnReport()
    {
        
        ModeratorActionOnReportVm moderatorActionOnReportVm = await GetDataSentByClient<ModeratorActionOnReportVm>();
        
        Console.WriteLine(moderatorActionOnReportVm.Action + " : " + moderatorActionOnReportVm.SubmissionUserId + " : " + moderatorActionOnReportVm.SubmissionEntryTime);
        
        
    }

    private async Task PopulatePageData(int? pageIndex)
    {
        List<ISearchCriterion> searchCriteria = new List<ISearchCriterion>()
        {
            new SearchCriterion<int>(SearchParameterTypes.HasNAmountReports, 1)
        };

        SubmissionDto submissionDto = await _searchService.GetSearchResults(searchCriteria);
        
        ModerationSubmissionMapper mapper = new ();
        _reports = mapper.ToVm(submissionDto);
        
        ItemsAmount = _reports.Count;
        
        DisplayItems =
            PaginatedList<ModerationSubmissionVm>.Create(
                _reports, 
                pageIndex?? 0,
                _configuration.GetValue("PageSize", 4));
    }
}