using Locompro.Common;
using Locompro.Common.Search;
using Locompro.Common.Search.QueryBuilder;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Common.Search.SearchQueryParameters;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;
using Locompro.Services.Domain;
using NetTopologySuite.Geometries;

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
    public async Task<SubmissionsDto> GetSearchSubmissionsAsync(ISearchQueryParameters<Submission> searchCriteria)
    {
        var submissions = await _submissionDomainService.GetByDynamicQuery(searchCriteria);
    
        return new SubmissionsDto(submissions, GetBestSubmission);
    }

    public async Task<SubmissionsDto> GetSearchResultsAsync(SearchVm searchVm)
    {   
        MapVm mapVm = new(searchVm.Latitude, searchVm.Longitude, searchVm.Distance);

        ISearchQueryParameters<Submission> searchParameters = new SearchQueryParameters<Submission>();
        searchParameters
            .AddQueryParameter(SearchParameterTypes.SubmissionByName, searchVm.ProductName)
            .AddQueryParameter(SearchParameterTypes.SubmissionByProvince, searchVm.ProvinceSelected)
            .AddQueryParameter(SearchParameterTypes.SubmissionByCanton, searchVm.CantonSelected)
            .AddQueryParameter(SearchParameterTypes.SubmissionByMinvalue, searchVm.MinPrice)
            .AddQueryParameter(SearchParameterTypes.SubmissionByMaxvalue, searchVm.MaxPrice)
            .AddQueryParameter(SearchParameterTypes.SubmissionByCategory, searchVm.CategorySelected)
            .AddQueryParameter(SearchParameterTypes.SubmissionByModel, searchVm.ModelSelected)
            .AddQueryParameter(SearchParameterTypes.SubmissionByBrand, searchVm.BrandSelected)
            .AddFilterParameter(SearchParameterTypes.SubmissionByLocationFilter, mapVm)
            /*
            .AddUniqueSearch(submission => MapVm.Ratio * submission.Store.Location.Distance(mapVm.Location) <= mapVm.Distance,
                mapVmParam => mapVmParam.Location != null && mapVmParam.Distance != 0,
                mapVm)*/;
        
        var submissions = await _submissionDomainService.GetByDynamicQuery(searchParameters);
    
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