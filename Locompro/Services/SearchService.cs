using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Repositories;
using Locompro.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Locompro.Services;

/// <summary>
/// Class that represents a single item to be displayed in the search results
/// An item is a product that is being sold in a store
/// </summary>
public class Item
{
    public string LastSubmissionDate { get; init; }
    public string ProductName { get; init; }
    public double ProductPrice { get; init; }
    public string ProductStore { get; init; }
    public string CantonLocation { get; init; }
    public string ProvinceLocation { get; init; }
    public string ProductDescription { get; init; }

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
    
    /// <summary>
    /// Gets submissions containing a specific product model
    /// </summary>
    public async Task<IEnumerable<Submission>> GetSubmissionsByProductModel(string productModel)
    {
        return await _submissionRepository.GetSubmissionsByProductModelAsync(productModel);
    }

    public async Task<IEnumerable<Item>> GetItems(List<Submission> submissions)
    {
        List<Item> items = new List<Item>();
        
        var submissionsByStore = submissions.GroupBy(s => s.Store);
        
        foreach (var store in submissionsByStore)
        {
            var submissionsByProduct = store.GroupBy(s => s.Product);
            foreach (var product in submissionsByProduct)
            {
                items.Add(await this.GetItem(product));
            }
        }

        return items;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemGrouping"></param>
    /// <returns></returns>
    public async Task<Item> GetItem (IGrouping<Product, Submission> itemGrouping)
    {
        // get best submission for its information
        Submission bestSubmission = this.GetBestSubmission(itemGrouping);
        
        // create an item
        Item item = new Item(
            bestSubmission.EntryTime.ToString(CultureInfo.InvariantCulture),
            bestSubmission.Product.Name,
            bestSubmission.Price,
            bestSubmission.Store.Name,
            bestSubmission.Store.Canton.Name,
            bestSubmission.Store.Canton.Province.Name,
            bestSubmission.Description
        );
        
        // add all submissions to list
        item.Submissions = itemGrouping.ToList();

        return await Task.FromResult(item);
    }
    
    public Submission GetBestSubmission(IEnumerable<Submission> submissions)
    {
        // for the time being, the best submission is the one with the most recent entry time
        return submissions.MaxBy(s => s.EntryTime);
    }
}
