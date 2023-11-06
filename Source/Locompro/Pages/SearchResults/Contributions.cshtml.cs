using System.Net;
using Castle.Core.Internal;
using Locompro.Common;
using Locompro.Common.Mappers;
using Locompro.Common.Search;
using Locompro.Common.Search.SearchMethodRegistration;
using Locompro.Models.Dtos;
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

        public async Task OnGetAsync(int? pageIndex, string query)
        {
            CurrentUserId = query;
            var searchParameters = new List<ISearchCriterion>
            {
              new SearchCriterion<string>(SearchParameterTypes.UserId, query),
            };
            List<ItemVm> searchResults = null;
            try
            {
                ItemMapper itemMapper = new();
                SubmissionDto submissionDto = await _searchService.GetSearchResults(searchParameters);
                searchResults = itemMapper.ToVm(submissionDto);
            }
            catch (Exception e)
            {
                Logger.LogError("Error when attempting to get search results: " + e.Message);
            }
            DisplayAmount = searchResults.Count;
            DisplaySubmissions = PaginatedList<ItemVm>.Create(searchResults, pageIndex ?? 1, _pageSize);
        }
    }
}
