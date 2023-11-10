using Locompro.Models.Entities;
using Locompro.Services;

namespace Locompro.Pages.Modals.AdvancedSearch;

/// <summary>
///     Data model used by the advanced search modal
/// </summary>
public class AdvancedSearchModalModel
{
    /// <summary>
    ///     Service that provides all province, canton and category data
    /// </summary>
    private readonly AdvancedSearchInputService advancedSearchService;

    public AdvancedSearchModalModel(AdvancedSearchInputService advancedSearchService)
    {
        this.advancedSearchService = advancedSearchService;
        EmptyValue = advancedSearchService.EmptyValue;
    }

    /// <summary>
    ///     Provinces shown in the advanced search modal
    /// </summary>
    public List<Province> provinces { get; set; }

    /// <summary>
    ///     Cantons shown in the advanced search modal
    /// </summary>
    public List<Canton> cantons { get; set; }

    /// <summary>
    ///     Categories shown in the advanced search modal
    /// </summary>
    public List<Category> categories { get; set; }

    public Category categorySelected { get; set; }

    /// <summary>
    ///     Province that was selected and used to select cantons to be shown
    /// </summary>
    public string provinceSelected { get; set; }

    /// <summary>
    ///     SubmissionByCanton that was selected
    /// </summary>
    public string cantonSelected { get; set; }

    public string EmptyValue { get; set; }

    /// <summary>
    ///     Sets the model cantons as the cantons loaded into the service by the province name
    /// </summary>
    /// <param name="provinceName"></param>
    /// <returns></returns>
    public async Task ObtainCantonsAsync(string provinceName)
    {
        await advancedSearchService.ObtainCantonsAsync(provinceName);
        cantons = advancedSearchService.Cantons;
    }

    /// <summary>
    ///     Sets the model provinces as the provinces loaded into the service
    /// </summary>
    /// <returns></returns>
    public async Task ObtainProvincesAsync()
    {
        await advancedSearchService.ObtainProvincesAsync();
        provinces = advancedSearchService.Provinces;
    }

    /// <summary>
    ///     Sets the model categories as the categories loaded into the service
    /// </summary>
    /// <returns></returns>
    public async Task ObtainCategoriesAsync()
    {
        await advancedSearchService.ObtainCategoriesAsync();
        categories = advancedSearchService.Categories;
    }
}