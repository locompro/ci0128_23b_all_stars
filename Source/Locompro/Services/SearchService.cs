using Locompro.Common;
using Locompro.Common.Search;
using Locompro.Common.Search.QueryBuilder;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services.Domain;

namespace Locompro.Services;

public class SearchService : Service, ISearchService
{
    public const int ImageAmountPerItem = 5;
    
    private readonly IDomainService<Submission, SubmissionKey> _submissionDomainService;

    /// <summary>
    ///     Constructor for the search service
    /// </summary>
    /// <param name="loggerFactory"> logger </param>
    /// <param name="submissionDomainService"></param>
    /// <param name="pictureService"></param>
    public SearchService(ILoggerFactory loggerFactory, IDomainService<Submission, SubmissionKey> submissionDomainService,
        IPictureService pictureService) :
        base(loggerFactory)
    {
        _submissionDomainService = submissionDomainService;
    }

    /// <summary>
    ///     Searches for items based on the criteria provided in the search view model.
    ///     This method aggregates results from multiple queries such as by product name, by product model, and by
    ///     canton/province.
    ///     It then returns a list of items that match all the criteria.
    /// </summary>
    public async Task<SubmissionsDto> GetSearchResults(ISearchQueryParameters<Submission> searchCriteria)
    {
        var submissions = await _submissionDomainService.GetByDynamicQuery(searchCriteria);

        return new SubmissionsDto(submissions, GetBestSubmission);
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