﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Locompro.Services;
using Locompro.Models;
using Castle.Core.Internal;

namespace Locompro.Pages.Modals.AdvancedSearch;

/// <summary>
///  Data model used by the advanced search modal
/// </summary>
public class AdvancedSearchModalModel
{
    /// <summary>
    /// Provinces shown in the advanced search modal
    /// </summary>
    public List<Province> provinces { get; set; }
    
    /// <summary>
    /// Cantons shown in the advanced search modal
    /// </summary>
    public List<Canton> cantons { get; set; }

    /// <summary>
    /// Categories shown in the advanced search modal
    /// </summary>
    public List<Category> categories { get; set; }
    
    public Category categorySelected { get; set; }

    /// <summary>
    /// Province that was selected and used to select cantons to be shown
    /// </summary>
    public string provinceSelected { get; set; }

    /// <summary>
    /// Canton that was selected
    /// </summary>
    public string cantonSelected { get; set; }
    
    public string EmptyValue { get; set; }

    /// <summary>
    /// Service that provides all province, canton and category data
    /// </summary>
    private AdvancedSearchInputService advancedSearchService;

    public AdvancedSearchModalModel(AdvancedSearchInputService advancedSearchService)
    {
        this.advancedSearchService = advancedSearchService;
        this.EmptyValue = advancedSearchService.EmptyValue;
    }

    /// <summary>
    /// Sets the model cantons as the cantons loaded into the service by the province name
    /// </summary>
    /// <param name="provinceName"></param>
    /// <returns></returns>
    public async Task ObtainCantonsAsync(string provinceName)
    {
        await this.advancedSearchService.ObtainCantonsAsync(provinceName);
        this.cantons = this.advancedSearchService.Cantons;
    }

    /// <summary>
    /// Sets the model provinces as the provinces loaded into the service
    /// </summary>
    /// <returns></returns>
    public async Task ObtainProvincesAsync()
    {
        await this.advancedSearchService.ObtainProvincesAsync();
        this.provinces = this.advancedSearchService.Provinces;
    }

    /// <summary>
    /// Sets the model categories as the categories loaded into the service
    /// </summary>
    /// <returns></returns>
    public async Task ObtainCategoriesAsync()
    {
        await this.advancedSearchService.ObtainCategoriesAsync(); 
        this.categories = this.advancedSearchService.Categories; 
    }
}

