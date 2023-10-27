using System.Globalization;
using Locompro.Models;
using System.Text.RegularExpressions;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class SearchService
{
    private readonly ISubmissionService _submissionService;

    /// <summary>
    /// Constructor for the search service
    /// </summary>
    /// <param name="submissionService"></param>
    public SearchService(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
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
    /// <param name="brand"></param>
    /// <returns>A list of items that match the search criteria.</returns>
    public async Task<List<Item>> SearchItems(string productName, string province, string canton, long minValue,
        long maxValue, string category, string model, string brand = null)
    {
        // List of items to be returned
        var items = new List<Item>();

        // List for submissions to be aggregated
        var submissions = new List<IEnumerable<Submission>>();

        if (!string.IsNullOrEmpty(productName))  // Results by product name
        {
            submissions.Add(await _submissionService.GetByProductName(productName));
        }

        if (!string.IsNullOrEmpty(model))  // Results by product model
        {
            submissions.Add(await _submissionService.GetByProductModel(model));
        }

        if (!string.IsNullOrEmpty(province))  // Results by canton and province
        {
            submissions.Add(await _submissionService.GetByCantonAndProvince(canton, province));
        }

        if (!string.IsNullOrEmpty(brand)) // Results by product brand
        {
            submissions.Add(await _submissionService.GetByBrand(brand));
        }

        // if there are no submissions
        if (submissions.Count == 0)
        {
            // just return an empty list
            return items;
        }

        // Look for intersection of all results
        var result = submissions.Aggregate((x, y) => x.Intersect(y));

        // Get items from the submissions
        items.AddRange(await GetItems(result));

        return items;
    }

    /// <summary>
    /// Gets all the items to be displayed in the search results
    /// from a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private static async Task<IEnumerable<Item>> GetItems(IEnumerable<Submission> submissions)
    {
        var items = new List<Item>();

        // Group submissions by store
        var submissionsByStore = submissions.GroupBy(s => s.Store);

        foreach (var store in submissionsByStore)
        {
            var submissionsByProduct = store.GroupBy(s => s.Product);
            foreach (var product in submissionsByProduct)
            {
                items.Add(await GetItem(product));
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
    private static async Task<Item> GetItem(IGrouping<Product, Submission> itemGrouping)
    {
        // Get best submission for its information
        var bestSubmission = GetBestSubmission(itemGrouping);

        var item = new Item(
            GetFormattedDate(bestSubmission),
            bestSubmission.Product.Name,
            bestSubmission.Price,
            bestSubmission.Store.Name,
            bestSubmission.Store.Canton.Name,
            bestSubmission.Store.Canton.Province.Name,
            bestSubmission.Description
        )
        {
            Submissions = itemGrouping.ToList(),
            Model = bestSubmission.Product.Model,
            Brand = bestSubmission.Product.Brand
        };

        return await Task.FromResult(item);
    }

    /// <summary>
    /// Extracts from entry time, the date in the format yyyy-mm-dd
    /// to be shown in the results page
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    private static string GetFormattedDate(Submission submission)
    {
        var regexMatch = Regex.Match(submission.EntryTime.ToString(CultureInfo.InvariantCulture), @"[0-9]*/[0-9.]*/[0-9]*");

        var date = regexMatch.Success ? regexMatch.Groups[0].Value : submission.EntryTime.ToString(CultureInfo.InvariantCulture);

        return date;
    }

    /// <summary>
    /// According to established heuristics determines best best submission
    /// from among a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private static Submission GetBestSubmission(IEnumerable<Submission> submissions)
    {
        // For the time being, the best submission is the one with the most recent entry time
        return submissions.MaxBy(s => s.EntryTime);
    }
}