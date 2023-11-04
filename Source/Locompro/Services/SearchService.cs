using System.Globalization;
using Locompro.Models;
using System.Text.RegularExpressions;
using Locompro.Common.Search;
using Locompro.Common.Search.Interfaces;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class SearchService : Service, ISearchService
{
    private readonly ISearchDomainService _searchDomainService;

    private readonly IQueryBuilder _queryBuilder;

    public const int ImageAmountPerItem = 5;

    /// <summary>
    /// Constructor for the search service
    /// </summary>
    /// <param name="unitOfWork"> generic unit of work</param>
    /// <param name="loggerFactory"> logger </param>
    /// <param name="searchDomainService"></param>
    public SearchService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory, ISearchDomainService searchDomainService, IPicturesService picturesService) :
        base(unitOfWork, loggerFactory)
    {
        _searchDomainService = searchDomainService;
        _queryBuilder = new QueryBuilder();
    }

    /// <summary>
    /// Searches for items based on the criteria provided in the search view model.
    /// This method aggregates results from multiple queries such as by product name, by product model, and by canton/province.
    /// It then returns a list of items that match all the criteria.
    /// </summary>
    public async Task<List<Item>> GetSearchResults(List<ISearchCriterion> unfilteredSearchCriteria)
    {
        // add the list of unfiltered search criteria to the query builder
        foreach (ISearchCriterion searchCriterion in unfilteredSearchCriteria)
        {
            try
            {
                this._queryBuilder.AddSearchCriterion(searchCriterion);
            }
            catch (ArgumentException exception)
            {
                // if the search criterion is invalid, report on it but continue execution
                this.Logger.LogWarning(exception.ToString());
            }
        }

        // compose the list of search functions
        SearchQueries searchQueries = this._queryBuilder.GetSearchFunction();

        if (searchQueries.IsEmpty)
        {
            return new List<Item>();
        }

        // get the submissions that match the search functions
        IEnumerable<Submission> submissions = await this._searchDomainService.GetSearchResults(searchQueries);

        _queryBuilder.Reset();

        if (!submissions.Any())
        {
            return new List<Item>();
        }

        IEnumerable<Item> items = await GetItems(submissions);

        return items.ToList();
    }

    /// <summary>
    /// Gets all the items to be displayed in the search results
    /// from a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private async Task<IEnumerable<Item>> GetItems(IEnumerable<Submission> submissions)
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
        
        List<string> categories = new List<string>();

        foreach (Submission submission in itemGrouping)
        {
            if (submission.Product.Categories == null)
            {
                continue;
            }
            categories.AddRange(submission.Product.Categories.Select(c => c.Name).ToList());
        }
        
        var item = new Item(
            bestSubmission,
            GetFormattedDate
        )
        {
            Submissions = GetDisplaySubmissions(itemGrouping.ToList()),
            Categories = categories
        };

        return await Task.FromResult(item);
    }
    
    /// <summary>
    /// Constructs a list of display submissions from a list of submissions
    /// Reduces the amount of memory necesary to display submissions
    /// </summary>
    /// <param name="submissions"> submissions to be turned into display submissions</param>
    /// <returns></returns>
    private static List<DisplaySubmission> GetDisplaySubmissions(List<Submission> submissions)
    {
        List<DisplaySubmission> displaySubmissions = new List<DisplaySubmission>();
        
        foreach (var submission in submissions)
        {
            displaySubmissions.Add(new DisplaySubmission(submission, GetFormattedDate));
        }

        return displaySubmissions;
    }

    /// <summary>
    /// Extracts from entry time, the date in the format yyyy-mm-dd
    /// to be shown in the results page
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    private static string GetFormattedDate(Submission submission)
    {
        // Define a timeout duration for the regex operation
        TimeSpan matchTimeout = TimeSpan.FromSeconds(2); // 1 second timeout

        // Use the Regex constructor that allows a timeout
        Regex regex = new Regex(@"[0-9]*/[0-9.]*/[0-9]*", RegexOptions.None, matchTimeout);

        // Perform the match with the timeout
        Match regexMatch = regex.Match(submission.EntryTime.ToString(CultureInfo.InvariantCulture));

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
    private static Submission GetBestSubmission(IEnumerable<Submission> submissions)
    {
        // For the time being, the best submission is the one with the most recent entry time
        return submissions.MaxBy(s => s.EntryTime);
    }
}