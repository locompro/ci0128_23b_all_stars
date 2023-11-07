using Locompro.Common;
using Locompro.Common.Search;
using Locompro.Common.Search.QueryBuilder;
using Locompro.Data;
using Locompro.Models.Dtos;
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
    /// <param name="loggerFactory"> logger </param>
    /// <param name="searchDomainService"></param>
    /// <param name="pictureService"></param>
    public SearchService(ILoggerFactory loggerFactory, ISearchDomainService searchDomainService,
        IPictureService pictureService) :
        base(loggerFactory)
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
    public async Task<SubmissionDto> GetSearchResults(List<ISearchCriterion> unfilteredSearchCriteria)
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

        if (searchQueries.IsEmpty) return new SubmissionDto(null, null);

        // get the submissions that match the search functions
        var submissions = await _searchDomainService.GetSearchResults(searchQueries);

        _queryBuilder.Reset();

        return new SubmissionDto(submissions, GetBestSubmission);
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