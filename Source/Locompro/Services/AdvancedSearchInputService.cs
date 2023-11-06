﻿using Locompro.Models.Entities;
using Locompro.Services.Domain;

namespace Locompro.Services;

/// <summary>
///     Service for the Advanced Search Modal
///     Helps get data from repositories and these from the database
///     Also helps keeping data available between caller page and the modal generated
/// </summary>
public class AdvancedSearchInputService
{
    /// <summary>
    ///     Service for fetching category data
    /// </summary>
    private readonly INamedEntityDomainService<Category, string> _categoryService;

    /// <summary>
    ///     Service for fetching location data
    /// </summary>
    private readonly INamedEntityDomainService<Country, string> _countryService;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="countryService"></param>
    /// <param name="categoryService"></param>
    public AdvancedSearchInputService(INamedEntityDomainService<Country, string> countryService,
        INamedEntityDomainService<Category, string> categoryService)
    {
        _countryService = countryService;
        _categoryService = categoryService;
    }

    /// <summary>
    ///     List of provinces
    /// </summary>
    public List<Province> Provinces { get; set; }

    /// <summary>
    ///     List of cantons
    /// </summary>
    public List<Canton> Cantons { get; set; }

    /// <summary>
    ///     List of categories
    /// </summary>
    public List<Category> Categories { get; set; }

    /// <summary>
    ///     Province that was selected
    /// </summary>
    public string ProvinceSelected { get; set; }

    public string EmptyValue { get; set; }

    /// <summary>
    ///     Sets service provinces to all provinces
    /// </summary>
    /// <returns></returns>
    public async Task ObtainProvincesAsync()
    {
        // get the country
        var country = await _countryService.Get("Costa Rica");
        // for the country, get all provinces
        Provinces = country.Provinces.ToList();
    }

    /// <summary>
    ///     Set service cantons to all cantons for a given province
    /// </summary>
    /// <param name="provinceName"></param>
    /// <returns></returns>
    public async Task ObtainCantonsAsync(string provinceName)
    {
        // get the country
        var country = await _countryService.Get("Costa Rica");

        // get the requested province
        var requestedProvince =
            country.Provinces.ToList().Find(province => province.Name == provinceName);

        // set the cantons to the cantons of the requested province
        Cantons = await Task.FromResult(requestedProvince.Cantons.ToList());
    }

    /// <summary>
    ///     Set service categories get all categories
    /// </summary>
    /// <returns></returns>
    public async Task ObtainCategoriesAsync()
    {
        Categories = (await _categoryService.GetAll()).ToList();
    }
}