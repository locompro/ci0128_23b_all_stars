using System.Globalization;
using Locompro.Repositories;
using Locompro.Models;

namespace Locompro.Services;

/// <summary>
/// Class that represents a single item to be displayed in the search results
/// An item is a product that is being sold in a store
/// </summary>
public class Item
{
    public string LastSubmissionDate { get; set; }
    public string ProductName { get; set; }
    public double ProductPrice { get; set; }
    public string ProductStore { get; set; }
    public string CantonLocation { get; set; }
    public string ProvinceLocation { get; set; }
    public string ProductDescription { get; set; }

    public List<Submission> Submissions { get; set; }

    public Item(string lastSubmissionDate,
        string productName,
        double productPrice,
        string productStore,
        string cantonLocation,
        string provinceLocation,
        string productDescription)
    {
        this.LastSubmissionDate = lastSubmissionDate;
        this.ProductName = productName;
        this.ProductPrice = productPrice;
        this.ProductStore = productStore;
        this.CantonLocation = cantonLocation;
        this.ProvinceLocation = provinceLocation;
        this.ProductDescription = productDescription;
    }
};

public class SearchService
{
    private readonly SubmissionRepository _submissionRepository;
  

    public SearchService(SubmissionRepository submissionRepository, CountryRepository countryRepository,
        ProductRepository productRepository)
    {
        _submissionRepository = submissionRepository;

    }
    public async Task<IEnumerable<Submission>> GetSubmissionByCanton(string canton, string province)
    {
        return await _submissionRepository.GetSubmissionsByCantonAsync(canton, province);
    }
}
