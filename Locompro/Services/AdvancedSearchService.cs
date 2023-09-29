using Locompro.Models;

namespace Locompro.Services
{
    // placeholder until actual model exists
    public class Category
    {
        public string Name { get; set; }
        public Category(string name) { Name = name; }
    };

    public class AdvancedSearchService
    {
        private AdministrativeUnitService administrativeUnitService;

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

        public AdvancedSearchService(AdministrativeUnitService administrativeUnitService)
        {
            this.administrativeUnitService = administrativeUnitService;
        }

        // make the model get all provinces
        public async Task fetchProvincesAsync()
        {
            provinces = (List<Province>)await administrativeUnitService.GetAllProvinces();
        }

        // make the model get all cantons for a given province
        public async Task fetchCantonsAsync(string provinceName)
        {
            cantons = (List<Canton>)await administrativeUnitService.GetCantons(provinceName);
        }

        // make the model get all categories
        public void fetchCategories()
        {

        }
    }
}
