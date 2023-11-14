using Locompro.Common.Search;
using Locompro.Models.Dtos;
using Locompro.Models.ViewModels;

namespace Locompro.Services.Domain;

public interface ISearchService
{
    Task<SubmissionsDto> GetSearchResults(List<ISearchCriterion> searchCriteria);
}