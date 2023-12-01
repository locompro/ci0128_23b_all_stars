using System.ComponentModel.DataAnnotations;
using Locompro.Models.Entities;

namespace Locompro.Models.ViewModels;

/// <summary>
///     An instance of a product sold in a store.
///     Functions as a container for submissions at a search level.
/// </summary>
public class ItemVm
{
    /// <summary>
    ///     Constructor for an item.
    /// </summary>
    /// <param name="bestSubmission"></param>
    /// <param name="getFormattedDate"></param>
    public ItemVm(
        Submission bestSubmission,
        Func<Submission, string> getFormattedDate)
    {
        LastSubmissionDate = getFormattedDate(bestSubmission);
        Name = bestSubmission.Product.Name ?? "";
        Price = bestSubmission.Price;
        Store = bestSubmission.Store.Name ?? "";
        Canton = bestSubmission.Store.Canton.Name ?? "";
        Province = bestSubmission.Store.Canton.Province.Name ?? "";
        Description = bestSubmission.Description ?? "";
        Model = bestSubmission.Product.Model ?? "";
        Brand = bestSubmission.Product.Brand ?? "";
        
    }

    public string LastSubmissionDate { get; init; }
    public string Name { get; init; }
    public double Price { get; init; }
    public string FormattedPrice => Price.ToString("C0").TrimStart('$');

    public string Store { get; init; }
    public string Canton { get; init; }
    public string Province { get; init; }
    public List<string> Categories { get; init; }
    
    public string Description { get; init; }

    [DisplayFormat(NullDisplayText = "N/A")]
    public string Model { get; init; }
    public string Brand { get; set; }
    public List<SubmissionVm> Submissions { get; set; }
}