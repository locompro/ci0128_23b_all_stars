using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Locompro.Repositories;
using Locompro.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.RegularExpressions;

namespace Locompro.Services;

public class searchParam
{
    public enum SearchParameterTypes
    {
        NAME,
        PROVINCE,
        CANTON,
        MINVALUE,
        MAXVALUE,
        CATEGORY,
        MODEL,
        BRAND
    }
    
    public Func<Submission, string, bool> SearchQuery { get; set; }
    public Func<string, bool> ActivationQualifier { get; set; }
}

public class SearchService
{
    private readonly SubmissionRepository _submissionRepository;

    private Dictionary<searchParam.SearchParameterTypes, searchParam> _searchParameters;

    /// <summary>
    /// Constructor for the search service
    /// </summary>
    /// <param name="submissionRepository"></param>
    public SearchService(SubmissionRepository submissionRepository)
    {
        _submissionRepository = submissionRepository;
        _searchParameters = new Dictionary<searchParam.SearchParameterTypes, searchParam>();
        this.SubscribeAllSearchParameters();
    }
    
    public void SubscribeNewSearchParameter(searchParam.SearchParameterTypes parameterName, Func<Submission, string, bool> searchQuery, Func<string, bool> activationQualifier)
    {
        _searchParameters.Add(parameterName, new searchParam { SearchQuery = searchQuery, ActivationQualifier = activationQualifier });
    }

    /// <summary>
    /// Placeholder
    /// </summary>
    public void SubscribeAllSearchParameters()
    {
        SubscribeNewSearchParameter(searchParam.SearchParameterTypes.NAME
            , (submission, productName) => submission.Product.Name.ToLower().Contains(productName.ToLower())
            , productName => !string.IsNullOrEmpty(productName));
        
        SubscribeNewSearchParameter(searchParam.SearchParameterTypes.MODEL
            , (submission, model) => submission.Product.Model.ToLower().Contains(model.ToLower())
            , model => !string.IsNullOrEmpty(model));
        
        SubscribeNewSearchParameter(searchParam.SearchParameterTypes.PROVINCE
            , (submission, province) => submission.Store.Canton.Province.Name.ToLower().Contains(province.ToLower())
            , province => !string.IsNullOrEmpty(province));
        
        SubscribeNewSearchParameter(searchParam.SearchParameterTypes.CANTON
            , (submission, canton) => submission.Store.Canton.Name.ToLower().Contains(canton.ToLower())
            , canton => !string.IsNullOrEmpty(canton));
        
        SubscribeNewSearchParameter(searchParam.SearchParameterTypes.BRAND
            , (submission, brand) => submission.Product.Brand.ToLower().Contains(brand.ToLower())
            , brand => !string.IsNullOrEmpty(brand));
    }

    /// <summary>
    /// Searches for items based on the criteria provided in the search view model.
    /// This method aggregates results from multiple queries such as by product name, by product model, and by canton/province.
    /// It then returns a list of items that match all the criteria.
    /// </summary>
    public async Task<List<Item>> GetSearchResults(List<(searchParam.SearchParameterTypes, string)> unfilteredSearchCriteria)
    {
        List<Func<Submission, bool>> searchCriteria = new List<Func<Submission, bool>>();
        
        FilterSearchCriteria(unfilteredSearchCriteria, searchCriteria);
        
        IEnumerable<Submission> submissions = await this._submissionRepository.GetSearchResults(searchCriteria);
        
        return this.GetItems(submissions).Result.ToList();
    }

    private void FilterSearchCriteria(
        List<(searchParam.SearchParameterTypes, string)> unfilteredSearchCriteria,
        List<Func<Submission, bool>> filteredSearchCriteriaTarget)
    {
        // for each of the criterion in the unfiltered list
        foreach ((searchParam.SearchParameterTypes, string) searchCriterion in unfilteredSearchCriteria)
        {
            // if not within the internal map
            if (!this._searchParameters.ContainsKey(searchCriterion.Item1))
            {
                // might be better to throw an exception here, for now just ignore
                continue;
            }
            
            // get the search parameter that corresponds to the criterion
            searchParam searchParameter = this._searchParameters[searchCriterion.Item1];
            
            // if according to its activation qualifier
            if (searchParameter.ActivationQualifier(searchCriterion.Item2))
            {
                // add its search query to the filtered list
                filteredSearchCriteriaTarget.Add(submission => searchParameter.SearchQuery(submission, searchCriterion.Item2));
            }
        }
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
        // For the time being, the best submission is the one with the most recent entry time
        return submissions.MaxBy(s => s.EntryTime);
    }
}