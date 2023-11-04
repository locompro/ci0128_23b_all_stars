using System.ComponentModel.DataAnnotations;
using Locompro.Models.ViewModels;

namespace Locompro.Models;

/// <summary>
/// An instance of a product sold in a store.
/// Functions as a container for submissions at a search level.
/// </summary>
public class Item
{
    public string LastSubmissionDate { get; init; }
    public string Name { get; init; }
    public double Price { get; init; }
    public string Store { get; init; }
    public string Canton { get; init; }
    public string Province { get; init; }
    public List<string> Categories { get; init; }
    public string Description { get; init; }

    [DisplayFormat(NullDisplayText = "N/A")]
    public string Model { get; init; }

    public string Brand { get; set; }
    public List<SubmissionViewModel> Submissions { get; set; }

    /// <summary>
    /// Constructor for an item.
    /// </summary>
    /// <param name="bestSubmission"></param>
    /// <param name="getFormattedDate"></param>
    public Item(
        Submission bestSubmission,
        Func<Submission, string> getFormattedDate)
    {
        this.LastSubmissionDate = getFormattedDate(bestSubmission);
        this.Name = bestSubmission.Product.Name;
        this.Price = bestSubmission.Price;
        this.Store = bestSubmission.Store.Name;
        this.Canton = bestSubmission.Store.Canton.Name;
        this.Province = bestSubmission.Store.Canton.Province.Name;
        this.Description = bestSubmission.Description;
        this.Model = bestSubmission.Product.Model;
        this.Brand = bestSubmission.Product.Brand;
    }
};