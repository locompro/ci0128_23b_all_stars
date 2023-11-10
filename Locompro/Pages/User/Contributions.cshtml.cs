using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Locompro.Services;
using Locompro.Models;
using Microsoft.Extensions.Configuration;
using Locompro.Common;

namespace Locompro.Pages.User.Contributions
{
    public class ContributionsPageModel : PageModel
    {
        private readonly SearchService _searchService;
        private readonly IConfiguration _configuration;
        private readonly int _pageSize;

        public PaginatedList<Item> DisplayItems { get; set; }
        public double ItemsAmount { get; set; }
        public string ProductName { get; set; }
        public string ProvinceSelected { get; set; }
        public string CantonSelected { get; set; }
        public string CategorySelected { get; set; }
        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }
        public string ModelSelected { get; set; }
        public string BrandSelected { get; set; }
        public string NameSort { get; set; }
        public string CurrentSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentUser { get; set; }


        public ContributionsPageModel(AdvancedSearchInputService advancedSearchServiceHandler,
            IConfiguration configuration,
            SearchService searchService)
        {
            this._searchService = searchService;
            this._configuration = configuration;
            this._pageSize = _configuration.GetValue("PageSize", 4);
        }

        public async Task OnGetAsync(int? pageIndex, string query)
        {
            this.CurrentUser = query; // Set the username to filter submissions

            // Modify the call to SearchItems to include the username for filtering
            List<Item> _items = (await _searchService.SearchItems(
                this.MinPrice,
                this.MaxPrice,
                null, // Remove the query parameter if it's not needed
                this.ProvinceSelected,
                this.CantonSelected,
                this.CategorySelected,
                this.ModelSelected,
                this.BrandSelected,
                this.CurrentUser)
            ).ToList();

            this.ItemsAmount = _items.Count;
            this.DisplayItems = PaginatedList<Item>.Create(_items, pageIndex ?? 1, _pageSize);
        }

    }
}
