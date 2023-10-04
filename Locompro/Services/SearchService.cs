using System.Collections.Generic;
using System.Threading.Tasks;
using Locompro.Repositories;
using Locompro.Models;

namespace Locompro.Services;

/// <summary>
/// Class that represents a single item to be displayed in the search results
/// An item is a product that is being sold in a store
/// </summary>
public class Item
{
    public string lastSubmissionDate { get; set; }
    public string productName { get; set; }
    public double productPrice { get; set; }
    public string productStore { get; set; }
    public string cantonLocation { get; set; }
    public string provinceLocation { get; set; }
    public string productDescription { get; set; }

    public Item(string lastSubmissionDate,
        string productName,
        double productPrice,
        string productStore,
        string cantonLocation,
        string provinceLocation,
        string productDescription)
    {
        this.lastSubmissionDate = lastSubmissionDate;
        this.productName = productName;
        this.productPrice = productPrice;
        this.productStore = productStore;
        this.cantonLocation = cantonLocation;
        this.provinceLocation = provinceLocation;
        this.productDescription = productDescription;
    }
};

public class SearchService
{
    private readonly SubmissionRepository _submissionRepository;
    private readonly ProductRepository _productRepository;
    private readonly CountryRepository _countryRepository;

    public SearchService(SubmissionRepository submissionRepository, CountryRepository countryRepository,
        ProductRepository productRepository)
    {
        _submissionRepository = submissionRepository;
        _countryRepository = countryRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> getProductsByName(string name)
    {
        return await _productRepository.getProductsByName(name);
    }
/*
public async Task<IEnumerable<Submission>> SearchSubmissions(string countryName, string productName)
{

    var country = await _countryRepository.Get(countryName);
    var product = await _productRepository.Get(productName);
    var submissions = await _submissionRepository.GetAll();
    return submissions.Where(s => s.Product == product && s.Store.Country == country);

} */
    
}