using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Locompro.Services;
using Locompro.Models;
using Microsoft.Extensions.Configuration;
using Locompro.Common;
using Locompro.Repositories.Utilities;

namespace Locompro.Pages.SearchResults
{
    public class ContributionsPageModel : PageModel
    {
        private readonly SearchService _searchService;
        private readonly IConfiguration _configuration;
        private readonly int _pageSize;

        public PaginatedList<Submission> DisplaySubmissions { get; set; }
        public double DisplayAmount { get; set; }
        public string ProductName { get; set; }
        public string NameSort { get; set; }
        public string CurrentSort { get; set; }
        public string CurrentUser { get; set; }


        public ContributionsPageModel(AdvancedSearchInputService advancedSearchServiceHandler,
            IConfiguration configuration,
            SearchService searchService)
        {
            _searchService = searchService;
            _configuration = configuration;
            _pageSize = _configuration.GetValue("PageSize", 4);
        }

        public async Task OnGetAsync(int? pageIndex, string query)
        {
            CurrentUser = query;

            List<SearchCriterion> searchParameters = new List<SearchCriterion>()
            {
            new SearchCriterion(SearchParam.SearchParameterTypes.Username, CurrentUser),
            };

            List<Item> _items = await _searchService.GetSearchResults(searchParameters);
            List<Submission> _submissions = _items
                .SelectMany(item => item.Submissions)
                .OrderBy(submission => submission.EntryTime)
                .ToList(); DisplayAmount = _submissions.Count;
            DisplaySubmissions = PaginatedList<Submission>.Create(_submissions, pageIndex ?? 1, _pageSize);
        }
    }
}
