using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

    public List<Submission> submissions { get; set; }

    public Item(string lastSubmissionDate,
        string productName,
        double productPrice,
        string productStore,
        string cantonLocation,
        string provinceLocation,
        string productDescription,
        List<Submission> submissions = null)
    {
        this.lastSubmissionDate = lastSubmissionDate;
        this.productName = productName;
        this.productPrice = productPrice;
        this.productStore = productStore;
        this.cantonLocation = cantonLocation;
        this.provinceLocation = provinceLocation;
        this.productDescription = productDescription;
        this.submissions = submissions;
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

    /// <summary>
    /// With list of products, returns list of items
    /// </summary>
    /// <param name="products"></param>
    /// <returns></returns>
    private async Task<IEnumerable<Item>> getItems(List<Product> products)
    {
        // make list to store items
        List < Item > items = new List<Item>();
        
        // for each product
        foreach (Product product in products)
        {
            // get the item and add it to the list
            items.Add(await this.getItemForProduct(product));
        }

        return items;
    }
    
    /// <summary>
    /// Returns a item object for a given product
    /// Includes the information from the best submission
    /// as well as all submissions for the given product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    private async Task<Item> getItemForProduct(Product product)
    {
        // get best submission for product
        Submission bestSubmission = await this._submissionRepository.getBestSubmission(product.Id);
        
        // get all submissions for product
        List<Submission> productSubmissions = (await this._submissionRepository.getSubmissionsForProduct(product.Id)).ToList();
        
        // create item to return
        Item item = new Item(
            bestSubmission.EntryTime.ToString(CultureInfo.CurrentCulture),
            product.Name,
            bestSubmission.Price,
            bestSubmission.Store.Name,
            bestSubmission.Store.Canton.Name,
            bestSubmission.Store.Canton.Province.Name,
            bestSubmission.Description,
            productSubmissions);

        return item;
    }
    public async Task<IEnumerable<Product>> getProductByModel(string model)
    {
        return await _productRepository.getByModelAsync(model);
    }

    public async Task<IEnumerable<Item>> searchItems(string name, string province, string canton, long minValue,
        long maxValue, string category, string model)
    {
        // Validate parameters
        // bool = validateParameters
        
        List<IEnumerable<Product>> products = new List<IEnumerable<Product>>();
        
        // query products by name
        if (name != null)
        {
            products.Add(await this.getProductsByName(name));
        }
        
        // query products by province and canton
        // if province is null, dont query anything
        // if canton is null and province is not null, query by province
        
        // query products by price
        // if max value is 0, query everything
        // if min value is 0, and max value is not 0, query everything less than max value
        
        // query products by category
        // if category is null, query everything
        
        // query products by model
        if (model != null)
        {
            products.Add(await this.getProductByModel(model));
        }
        
        // get intersection of all products
        var intersection = products.Aggregate((previous, next) => previous.Intersect(next));
        
        // get items for intersection
        var items = await this.getItems(intersection.ToList());
        
        return items;
    } 
    
}