using Locompro.Models;

namespace Locompro.Services
{
    /// <summary>
    /// Service for the Advanced Search Modal
    /// Helps get data from repositories and these from the database
    /// Also helps keeping data available between caller page and the modal generated
    /// </summary>
    public class AdvancedSearchModalService
    {
        /// <summary>
        /// Service for fetching location data
        /// </summary>
        private CountryService countryService;

        /// <summary>
        /// Service for fetching category data
        /// </summary>
        private CategoryService categoryService;

        /// <summary>
        /// List of provinces
        /// </summary>
        public List<Province> provinces { get; set; }

        /// <summary>
        /// List of cantons
        /// </summary>
        public List<Canton> cantons { get; set; }

        /// <summary>
        /// List of categories
        /// </summary>
        public List<Category> categories { get; set; }

        /// <summary>
        /// Province that was selected
        /// </summary>
        public string provinceSelected { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="countryService"></param>
        /// <param name="categoryService"></param>
        public AdvancedSearchModalService(CountryService countryService, CategoryService categoryService)
        {
            this.countryService = countryService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Sets service provinces to all provinces
        /// </summary>
        /// <returns></returns>
        public async Task ObtainProvincesAsync()
        {
            // get the country
            Country country = await countryService.Get("Costa Rica");
            // for the country, get all provinces
            provinces = country.Provinces.ToList();
        }

        /// <summary>
        /// Set service cantons to all cantons for a given province
        /// </summary>
        /// <param name="provinceName"></param>
        /// <returns></returns>
        public async Task ObtainCantonsAsync(string provinceName)
        {
            // get the country
            Country country = await countryService.Get("Costa Rica");

            // get the requested province
            Province requestedProvince =
                country.Provinces.ToList().Find(province => province.Name == provinceName);
           
            // set the cantons to the cantons of the requested province
            cantons = await Task.FromResult(requestedProvince.Cantons.ToList());
        }

        /// <summary>
        /// Set service categories get all categories
        /// </summary>
        /// <returns></returns>
        public async Task ObtainCategoriesAsync()
        {
            this.categories = (await this.categoryService.GetAll()).ToList();
 
        }
    }
}
