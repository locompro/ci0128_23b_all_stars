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
    private readonly ProductRepository _productRepository;
    private readonly CountryRepository _countryRepository;

    public SearchService(SubmissionRepository submissionRepository, CountryRepository countryRepository,
        ProductRepository productRepository)
    {
        _submissionRepository = submissionRepository;
        _countryRepository = countryRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return await _productRepository.getProductsByName(name);
    }
    
    public async Task<IEnumerable<Submission>> GetSubmissionByCanton(string canton, string province)
    {
        return await _submissionRepository.GetSubmissionsByCantonAsync(canton, province);
    }

    /// <summary>
    /// With list of products, returns list of items
    /// </summary>
    /// <param name="products"></param>
    /// <returns></returns>
    private async Task<IEnumerable<Item>> GetItems(List<Product> products)
    {
        // make list to store items
        List < Item > items = new List<Item>();
        
        // for each product
        foreach (Product product in products)
        {
            // get the item and add it to the list
            items.Add(await this.GetItemForProduct(product));
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
    private async Task<Item> GetItemForProduct(Product product)
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
            bestSubmission.Description);
        item.Submissions = productSubmissions;

        return item;
    }

    private async Task<IEnumerable<Product>> GetProductByModel(string model)
    {
        return await _productRepository.getByModelAsync(model);
    }

    public async Task<IEnumerable<Item>> SearchItems(string name, string province, string canton, long minValue,
        long maxValue, string category, string model)
    {
        // Validate parameters
        // bool = validateParameters
        
        List<IEnumerable<Product>> products = new List<IEnumerable<Product>>();
        
        // query products by name
        if (name != null)
        {
            products.Add(await this.GetProductsByName(name));
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
            products.Add(await this.GetProductByModel(model));
        }
       
        var intersection = products.Skip(1).Aggregate(products[0], (previous, next) => previous.Intersect(next)).ToList();

        // get items for intersection
        var items = await this.GetItems(intersection);

        return items;

       
        
    } 
    
}
