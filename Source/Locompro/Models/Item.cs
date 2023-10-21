namespace Locompro.Models;

/// <summary>
/// An instance of a product sold in a store.
/// Functions as a container for submissions at a search level.
/// </summary>
public class Item
{
    /// <summary>
    /// Constructor for an item.
    /// </summary>
    /// <param name="lastSubmissionDate">Date of the last submission for this item.</param>
    /// <param name="name">Name of the product this item represents.</param>
    /// <param name="price">Price from the last submission for this item.</param>
    /// <param name="store">Store containing the item product.</param>
    /// <param name="canton">Canton where item store is located.</param>
    /// <param name="province">Province where item store is located.</param>
    /// <param name="description">Description of the last submission for this item.</param>
    /// <param name="model">Model of the product</param>
    public Item(string lastSubmissionDate,
        string name,
        double price,
        string store,
        string canton,
        string province,
        string description,
        string model)
    {
        this.LastSubmissionDate = lastSubmissionDate;
        this.Name = name;
        this.Price = price;
        this.Store = store;
        this.Canton = canton;
        this.Province = province;
        this.Description = description;
        this.Model = model;
    }
    
    public string LastSubmissionDate { get; init; }
    public string Name { get; init; }
    public double Price { get; init; }
    public string Store { get; init; }
    public string Canton { get; init; }
    public string Province { get; init; }
    public string Description { get; init; }

    public string Model { get; init; }

    public string Brand { get; set; }
    public List<Submission> Submissions { get; set; }
};