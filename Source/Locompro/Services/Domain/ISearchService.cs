using Locompro.Common.Search;
using Locompro.Models;
using Locompro.Models.ViewModels;

namespace Locompro.Services.Domain;

public interface ISearchService
{
    Task<List<ItemVm>> GetSearchResults(List<ISearchCriterion> unfilteredSearchCriteria);
}