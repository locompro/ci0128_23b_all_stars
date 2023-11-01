using Locompro.Models;
using Locompro.SearchQueryConstruction;

namespace Locompro.Services.Domain;

public interface ISearchService
{
    Task<List<Item>> GetSearchResults(List<ISearchCriterion> unfilteredSearchCriteria);
}