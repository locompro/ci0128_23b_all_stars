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

    /// <summary>
    /// Constructor for an item
    /// </summary>
    /// <param name="lastSubmissionDate"></param>
    /// <param name="productName"></param>
    /// <param name="productPrice"></param>
    /// <param name="productStore"></param>
    /// <param name="cantonLocation"></param>
    /// <param name="provinceLocation"></param>
    /// <param name="productDescription"></param>
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

    /// <summary>
    /// Constructor for the search service
    /// </summary>
    /// <param name="submissionRepository"></param>
    /// <param name="countryRepository"></param>
    /// <param name="productRepository"></param>
    public SearchService(SubmissionRepository submissionRepository, CountryRepository countryRepository,
        ProductRepository productRepository)
    {
        _submissionRepository = submissionRepository;
    }

    /// <summary>
    /// Searches for items based on the criteria provided in the search view model.
    /// This method aggregates results from multiple queries such as by product name, by product model, and by canton/province.
    /// It then returns a list of items that match all the criteria.
    /// </summary>
    /// <param name="productName"></param>
    /// <param name="province"></param>
    /// <param name="canton"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="category"></param>
    /// <param name="model"></param>
    /// <returns>A list of items that match the search criteria.</returns>
    public async Task<List<Item>> SearchItems(string productName, string province, string canton, long minValue,
        long maxValue, string category, string model)
    {
        
        // List of items to be returned
        List<Item> items = new List<Item>();
        
        // List for submissions to be aggregated
        List<IEnumerable<Submission>> submissions = new List<IEnumerable<Submission>>();

        // add results from searching by product name
        if (!string.IsNullOrEmpty(productName))
        {
            submissions.Add(await GetSubmissionsByProductName(productName));
        }
        
        // add results from searching by product model
        if (!string.IsNullOrEmpty(model))
        {
            submissions.Add(await GetSubmissionsByProductModel(model));
        }
        
        // add results from searching by canton and province
        if (!string.IsNullOrEmpty(canton) && !string.IsNullOrEmpty(province))
        {
            submissions.Add(await GetSubmissionsByCantonAndProvince(canton, province));
        }
        
        // aggregate results (look for intersection of all results)
        IEnumerable<Submission> result = submissions.Aggregate((x, y) => x.Intersect(y));
        
        // get items from the submissions
        items.AddRange(await GetItems(result));

        return items;
    }
    
    /// <summary>
    /// Gets submissions containing a specific product name
    /// </summary>
    /// <param name="productName"></param>
    /// <returns></returns>
    private async Task<IEnumerable<Submission>> GetSubmissionsByProductName(string productName)
    {
        return await _submissionRepository.GetSubmissionsByProductNameAsync(productName);
    }
    
    /// <summary>
    /// Gets submissions containing a specific product model
    /// </summary>
    private async Task<IEnumerable<Submission>> GetSubmissionsByProductModel(string productModel)
    {
        return await _submissionRepository.GetSubmissionsByProductModelAsync(productModel);
    }

    private async Task<IEnumerable<Submission>> GetSubmissionsByCantonAndProvince(string canton, string province)
    {
        return await _submissionRepository.GetSubmissionsByCantonAsync(canton, province);
    }
    
    public async Task<IEnumerable<Submission>> GetSubmissionByCanton(string canton, string province)
    {
        return await _submissionRepository.GetSubmissionsByCantonAsync(canton, province);
    }
    
    /// <summary>
    /// Gets all the items to be displayed in the search results
    /// from a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private async Task<IEnumerable<Item>> GetItems(IEnumerable<Submission> submissions)
    {
        // list to contain the items
        List<Item> items = new List<Item>();
        
        // group submissions by store
        IEnumerable<IGrouping<Store,Submission>> submissionsByStore = submissions.GroupBy(s => s.Store);

        // for each store
        foreach (IGrouping<Store,Submission> store in submissionsByStore)
        {
            // group submissions by product
            IEnumerable<IGrouping<Product,Submission>> submissionsByProduct = store.GroupBy(s => s.Product);
            
            // for each product
            foreach (IGrouping<Product,Submission> product in submissionsByProduct)
            {
                // add it as an item to the list
                items.Add(await this.GetItem(product));
            }
        }

        return items;
    }

    /// <summary>
    /// Produces an item from a group of submissions
    /// Gets the best submission from the group of items
    /// uses its information for the item to be shown
    /// </summary>
    /// <param name="itemGrouping"></param>
    /// <returns></returns>
    private async Task<Item> GetItem(IGrouping<Product, Submission> itemGrouping)
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
        )
        {
            // add all submissions to list
            Submissions = itemGrouping.ToList()
        };

        return await Task.FromResult(item);
    }
    
    /// <summary>
    /// According to established heuristics determines best best submission
    /// from among a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private Submission GetBestSubmission(IEnumerable<Submission> submissions)
    {
        // for the time being, the best submission is the one with the most recent entry time
        return submissions.MaxBy(s => s.EntryTime);
    }
}