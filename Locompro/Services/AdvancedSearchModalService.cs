using Locompro.Models;

namespace Locompro.Services
{
    // placeholder until actual model exists
    public class Category
    {
        public string Name { get; set; }
        public Category(string name) { Name = name; }
    };

    public class AdvancedSearchModalService
    {
        private CountryService countryService;

        public List<Province> provinces { get; set; }

        public List<Canton> cantons { get; set; }

        public string provinceSelected { get; set; }

        public List<Category> categories { get; set; } =
            new List<Category>
            {
                new Category("sombrero"),
                new Category("zapatos"),
                new Category("camisas"),
                new Category("pantalones")
            };

        public AdvancedSearchModalService(CountryService countryService)
        {
            this.countryService = countryService;
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
        public void ObtainCategories()
        {

        }
    }
}
