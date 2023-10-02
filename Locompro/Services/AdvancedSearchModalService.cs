using Locompro.Models;

namespace Locompro.Services
{
    // placeholder until actual model exists

    public class AdvancedSearchModalService
    {
        private CountryService countryService;

        private CategoryService categoryService;

        public List<Province> provinces { get; set; }

        public List<Canton> cantons { get; set; }

        public string provinceSelected { get; set; }

        public List<Category> categories { get; set; }

        public AdvancedSearchModalService(CountryService countryService, CategoryService categoryService)
        {
            this.countryService = countryService;
            this.categoryService = categoryService;
        }

        // set service provinces to all provinces
        public async Task ObtainProvincesAsync()
        {
            // get the country
            Country country = await countryService.Get("Costa Rica");
            // for the country, get all provinces
            provinces = country.Provinces.ToList();
        }

        // set service cantons to all cantons for a given province
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

        // set service categories get all categories
        public async Task ObtainCategoriesAsync()
        {
            this.categories = (await this.categoryService.GetAll()).ToList();

            foreach(Category category in this.categories)
            {
                Console.WriteLine(category.Name);
            }
        }
    }
}
