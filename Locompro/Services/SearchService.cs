using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Repositories;
using Locompro.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.RegularExpressions;

namespace Locompro.Services;

public class SearchService
{
    private readonly SubmissionRepository _submissionRepository;

    /// <summary>
    /// Constructor for the search service
    /// </summary>
    /// <param name="submissionRepository"></param>
    /// <param name="countryRepository"></param>
    /// <param name="productRepository"></param>
    public SearchService(SubmissionRepository submissionRepository)
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
    /// <param name="brand"></param>
    /// <returns>A list of items that match the search criteria.</returns>
    public async Task<List<Item>> SearchItems(string productName, string province, string canton, long minValue,
        long maxValue, string category, string model, string brand = null)
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
        
        // add results from searching by brand
        if (!string.IsNullOrEmpty(brand))
        {
            submissions.Add(await GetSubmissionsByBrand(brand));
        }
        
        // if there are no submissions
        if (submissions.Count == 0)
        {
            // just return an empty list
            return items;
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
    /// <remarks> This is just a wrapper for the submission repository </remarks>
    private async Task<IEnumerable<Submission>> GetSubmissionsByProductModel(string productModel)
    {
        return await _submissionRepository.GetSubmissionsByProductModelAsync(productModel);
    }
    
    /// <summary>
    /// Calls the submission repository to get all submissions containing a specific brand name
    /// </summary>
    /// <param name="brandName"></param>
    /// <returns> An Enumerable with al the submissions tha meet the criteria</returns>
    private async Task<IEnumerable<Submission>> GetSubmissionsByBrand(string brandName)
    {
        return await _submissionRepository.GetSubmissionByBrandAsync(brandName);
    }
 
    public async Task<IEnumerable<Submission>> GetSubmissionsByCantonAndProvince(string canton, string province)
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
            GetFormatedDate(bestSubmission),
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
    /// Extracts from entry time, the date in the format yyyy-mm-dd
    /// to be shown in the results page
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    string GetFormatedDate(Submission submission)
    {
        Match regexMatch = Regex.Match(submission.EntryTime.ToString(CultureInfo.InvariantCulture), @"[0-9]*/[0-9.]*/[0-9]*");

        string date = regexMatch.Success ? regexMatch.Groups[0].Value : submission.EntryTime.ToString(CultureInfo.InvariantCulture);
        
        return date;
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