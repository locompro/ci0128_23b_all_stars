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
        private List<Item> _items;
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

        public async Task OnGetAsync(int? pageIndex,
            bool? sorting,
            string query,
            string province,
            string canton,
            long minValue,
            long maxValue,
            string category,
            string model,
            string brand,
            string currentFilter,
            string sortOrder,
            string username) // Add the username parameter
        {
            this.ProductName = query;
            this.SetSortingParameters(sortOrder, (sorting is not null));

            // Modify the call to SearchItems to include the username
            this._items =
                (await _searchService.SearchItems(
                    this.MinPrice,
                    this.MaxPrice,
                    this.ProductName,
                    this.ProvinceSelected,
                    this.CantonSelected,
                    this.CategorySelected,
                    this.ModelSelected,
                    this.BrandSelected,
                    username)
                ).ToList();

            this.ItemsAmount = _items.Count;
            this.OrderItems();
            this.DisplayItems = PaginatedList<Item>.Create(_items, pageIndex ?? 1, _pageSize);
        }


        private void SetSortingParameters(string sortOrder, bool sorting)
        {
            if (!sorting)
            {
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    this.NameSort = sortOrder;
                }
                return;
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                this.NameSort = "name_asc";
            }
            else
            {
                this.NameSort = sortOrder.Contains("name_asc") ? "name_desc" : "name_asc";
            }
        }

        private void OrderItems()
        {
            switch (this.NameSort)
            {
                case "name_desc":
                    _items = _items.OrderByDescending(item => item.ProductName).ToList();
                    break;
                case "name_asc":
                    _items = _items.OrderBy(item => item.ProductName).ToList();
                    break;
            }
        }
    }
}
