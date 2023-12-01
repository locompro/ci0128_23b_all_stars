using Locompro.Common.Search;
using Locompro.Models.Dtos;
using Locompro.Models.Entities;
using Locompro.Models.ViewModels;

namespace Locompro.Services.Domain;

public interface ISearchService
{
    Task<SubmissionsDto> GetSearchSubmissionsAsync(ISearchQueryParameters<Submission> searchCriteria);
    
    Task<SubmissionsDto> GetSearchResultsAsync(SearchVm searchVm);
}