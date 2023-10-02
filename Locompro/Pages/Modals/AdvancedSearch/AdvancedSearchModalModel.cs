using Locompro.Services;
using Locompro.Models;
using Castle.Core.Internal;

namespace Locompro.Pages.Modals.AdvancedSearch;

public class AdvancedSearchModalModel
{
    public List<Province> provinces { get; set; }

    public List<Canton> cantons { get; set; }

    public string provinceSelected { get; set; }

    public string cantonSelected { get; set; }

    public List<Category> categories { get; set; }

    private AdvancedSearchModalService advancedSearchService;

    public AdvancedSearchModalModel(AdvancedSearchModalService advancedSearchService)
    {
        this.advancedSearchService = advancedSearchService;
    }

    public async Task ObtainCantonsAsync(string provinceName)
    {
        await this.advancedSearchService.ObtainCantonsAsync(provinceName);
        this.cantons = this.advancedSearchService.cantons;
    }

    public async Task ObtainProvincesAsync()
    {
        await this.advancedSearchService.ObtainProvincesAsync();
        this.provinces = this.advancedSearchService.provinces;
    }

    public void ObtainCategoriesAsync()
    {
        this.categories = this.advancedSearchService.categories;
    }
}

