using System.Net;
using Castle.Core.Internal;
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
using Locompro.Services;


namespace Locompro.Pages.SearchResults
{
    public class ContributionsPageModel : BasePageModel
    {
        private readonly SearchService _searchService;
        private readonly IConfiguration _configuration;
        private readonly int _pageSize;

        public PaginatedList<ItemVm> DisplaySubmissions { get; set; }
        public double DisplayAmount { get; set; }
        public string CurrentUserId { get; set; }


        public ContributionsPageModel(ILoggerFactory loggerFactory, 
                                    IHttpContextAccessor httpContextAccessor, 
                                    IConfiguration configuration, 
                                    SearchService searchService)
                                    : base(loggerFactory, httpContextAccessor)
        {
            _searchService = searchService;
            _configuration = configuration;
            _pageSize = _configuration.GetValue("PageSize", 4);
        }

        // Reacts to the current user and gives all the submissions done by them
        public async Task OnGetAsync(int? pageIndex, string query)
        {
            CurrentUserId = query;
            
            ISearchQueryParameters<Submission> searchParameters = new SearchQueryParameters<Submission>();
            searchParameters.AddQueryParameter(SearchParameterTypes.SubmissionByUserId, query);
            
            List<ItemVm> searchResults = null;
            
            try
            {
                ItemMapper itemMapper = new();
                SubmissionsDto submissionDto = await _searchService.GetSearchResults(searchParameters);
                searchResults = itemMapper.ToVm(submissionDto);
            }
            catch (Exception e)
            {
                Logger.LogError("Error when attempting to get search results: " + e.Message);
            }
            foreach (var itemVm in searchResults)
            {
                DisplayAmount += itemVm.Submissions.Count;
            }
            DisplaySubmissions = PaginatedList<ItemVm>.Create(searchResults, pageIndex ?? 1, _pageSize);
        }
    }
}
