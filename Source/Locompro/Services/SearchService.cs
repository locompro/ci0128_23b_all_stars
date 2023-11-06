using Locompro.Common;
using Locompro.Common.Search;
using Locompro.Common.Search.QueryBuilder;
using Locompro.Data;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class SearchService : Service, ISearchService
{
    public const int ImageAmountPerItem = 5;

    private readonly IQueryBuilder _queryBuilder;
    private readonly ISearchDomainService _searchDomainService;

    /// <summary>
    ///     Constructor for the search service
    /// </summary>
    /// <param name="unitOfWork"> generic unit of work</param>
    /// <param name="loggerFactory"> logger </param>
    /// <param name="searchDomainService"></param>
    public SearchService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory, ISearchDomainService searchDomainService,
        IPictureService pictureService) :
        base(unitOfWork, loggerFactory)
    {
        _searchDomainService = searchDomainService;
        _queryBuilder = new QueryBuilder();
    }

    /// <summary>
    ///     Searches for items based on the criteria provided in the search view model.
    ///     This method aggregates results from multiple queries such as by product name, by product model, and by
    ///     canton/province.
    ///     It then returns a list of items that match all the criteria.
    /// </summary>
    public async Task<List<ItemVm>> GetSearchResults(List<ISearchCriterion> unfilteredSearchCriteria)
    {
        // add the list of unfiltered search criteria to the query builder
        foreach (var searchCriterion in unfilteredSearchCriteria)
            try
            {
                _queryBuilder.AddSearchCriterion(searchCriterion);
            }
            catch (ArgumentException exception)
            {
                // if the search criterion is invalid, report on it but continue execution
                Logger.LogWarning(exception.ToString());
            }

        // compose the list of search functions
        var searchQueries = _queryBuilder.GetSearchFunction();

        if (searchQueries.IsEmpty) return new List<ItemVm>();

        // get the submissions that match the search functions
        var submissions = await _searchDomainService.GetSearchResults(searchQueries);

        _queryBuilder.Reset();

        if (!submissions.Any()) return new List<ItemVm>();

        var items = await GetItems(submissions);

        return items.ToList();
    }

    /// <summary>
    ///     Gets all the items to be displayed in the search results
    ///     from a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private static async Task<IEnumerable<ItemVm>> GetItems(IEnumerable<Submission> submissions)
    {
        var items = new List<ItemVm>();

        // Group submissions by store
        var submissionsByStore = submissions.GroupBy(s => s.Store);

        foreach (var store in submissionsByStore)
        {
            var submissionsByProduct = store.GroupBy(s => s.Product);
            foreach (var product in submissionsByProduct) items.Add(await GetItem(product));
        }

        return items;
    }

    /// <summary>
    ///     Produces an item from a group of submissions
    ///     Gets the best submission from the group of items
    ///     uses its information for the item to be shown
    /// </summary>
    /// <param name="itemGrouping"></param>
    /// <returns></returns>
    private static async Task<ItemVm> GetItem(IGrouping<Product, Submission> itemGrouping)
    {
        // Get best submission for its information
        var bestSubmission = GetBestSubmission(itemGrouping);

        var categories = new List<string>();

        foreach (var submission in itemGrouping)
        {
            if (submission.Product.Categories == null) continue;
            categories.AddRange(submission.Product.Categories.Select(c => c.Name).ToList());
        }

        var item = new ItemVm(
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
    ///     Constructs a list of display submissions from a list of submissions
    ///     Reduces the amount of memory necesary to display submissions
    /// </summary>
    /// <param name="submissions"> submissions to be turned into display submissions</param>
    /// <returns></returns>
    private static List<SubmissionVm> GetDisplaySubmissions(List<Submission> submissions)
    {
        var displaySubmissions = new List<SubmissionVm>();

        foreach (var submission in submissions) displaySubmissions.Add(new SubmissionVm(submission, GetFormattedDate));

        return displaySubmissions;
    }

    /// <summary>
    ///     Extracts from entry time, the date in the format yyyy-mm-dd
    ///     to be shown in the results page
    /// </summary>
    /// <param name="submission"></param>
    /// <returns></returns>
    private static string GetFormattedDate(Submission submission)
    {
        return DateFormatter.GetFormattedDateFromDateTime(submission.EntryTime);
    }

    /// <summary>
    ///     According to established heuristics determines best best submission
    ///     from among a list of submissions
    /// </summary>
    /// <param name="submissions"></param>
    /// <returns></returns>
    private static Submission GetBestSubmission(IEnumerable<Submission> submissions)
    {
        // For the time being, the best submission is the one with the most recent entry time
        return submissions.MaxBy(s => s.EntryTime);
    }
}