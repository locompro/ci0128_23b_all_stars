namespace Locompro.Models;

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