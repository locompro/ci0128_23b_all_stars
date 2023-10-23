using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Repositories;
using Locompro.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.RegularExpressions;
using Locompro.Repositories.Utilities;

namespace Locompro.Services;

public class SearchService
{
    private readonly SubmissionRepository _submissionRepository;

    private readonly QueryBuilder _queryBuilder;

    /// <summary>
    /// Constructor for the search service
    /// </summary>
    /// <param name="submissionRepository"></param>
    /// <param name="countryRepository"></param>
    /// <param name="productRepository"></param>
    public SearchService(SubmissionRepository submissionRepository)
    {
        _submissionRepository = submissionRepository;
        _queryBuilder = new QueryBuilder();
    }

    /// <summary>
    /// Searches for items based on the criteria provided in the search view model.
    /// This method aggregates results from multiple queries such as by product name, by product model, and by canton/province.
    /// It then returns a list of items that match all the criteria.
    /// </summary>
    public async Task<List<Item>> GetSearchResults(List<SearchCriterion> unfilteredSearchCriteria)
    {
        // add the list of unfiltered search criteria to the query builder
        foreach (SearchCriterion searchCriterion in unfilteredSearchCriteria)
        {
            this._queryBuilder.AddSearchCriterion(searchCriterion);
        }

        // compose the list of search functions
        SearchQuery searchQuery = this._queryBuilder.GetSearchFunction();
        
        // get the submissions that match the search functions
        IEnumerable<Submission> submissions = 
            searchQuery.IsEmpty?
                Enumerable.Empty<Submission>() :
                await this._submissionRepository.GetSearchResults(searchQuery);
        
        this._queryBuilder.Reset();
        
        return this.GetItems(submissions).Result.ToList();
    }
    
    /// <summary>
    /// Gets all the items to be displayed in the search results
    /// from a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private async Task<IEnumerable<Item>> GetItems(IEnumerable<Submission> submissions)
    {
        List<Item> items = new List<Item>();

        // Group submissions by store
        IEnumerable<IGrouping<Store, Submission>> submissionsByStore = submissions.GroupBy(s => s.Store);

        foreach (IGrouping<Store, Submission> store in submissionsByStore)
        {
            IEnumerable<IGrouping<Product, Submission>> submissionsByProduct = store.GroupBy(s => s.Product);
            foreach (IGrouping<Product, Submission> product in submissionsByProduct)
            {
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
        // Get best submission for its information
        Submission bestSubmission = this.GetBestSubmission(itemGrouping);

        Item item = new Item(
            GetFormatedDate(bestSubmission),
            bestSubmission.Product.Name,
            bestSubmission.Price,
            bestSubmission.Store.Name,
            bestSubmission.Store.Canton.Name,
            bestSubmission.Store.Canton.Province.Name,
            bestSubmission.Description,
            bestSubmission.Product.Model
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
    string GetFormatedDate(Submission submission)
    {
        Match regexMatch = Regex.Match(submission.EntryTime.ToString(CultureInfo.InvariantCulture),
            @"[0-9]*/[0-9.]*/[0-9]*");

        string date = regexMatch.Success
            ? regexMatch.Groups[0].Value
            : submission.EntryTime.ToString(CultureInfo.InvariantCulture);

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
        // For the time being, the best submission is the one with the most recent entry time
        return submissions.MaxBy(s => s.EntryTime);
    }
}