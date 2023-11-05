using Locompro.Common.Search;
using Locompro.Models;

namespace Locompro.Services.Domain;

public interface ISearchService
{
    Task<List<Item>> GetSearchResults(List<ISearchCriterion> unfilteredSearchCriteria);
}